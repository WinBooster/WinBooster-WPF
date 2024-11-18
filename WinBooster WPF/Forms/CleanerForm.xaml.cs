using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WinBooster_WPF.Forms;
using WinBoosterNative;
using WinBoosterNative.database.cleaner;
using WinBoosterNative.database.cleaner.workers.custom;

namespace WinBooster_WPF
{
    public partial class CleanerForm : HandyControl.Controls.Window
    {
        private Dictionary<string, CheckBox> checkBoxes = new Dictionary<string, CheckBox>();
        public CleanerForm()
        {
            InitializeComponent();
            App.UpdateScreenCapture(this);
            clearListForm = new ClearListForm();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private enum NumberSuffix
        {
            P,
            K,
            M,
            B,
            T,
            Q
        }
        public static string FormatNumber(long numberToFormat, int decimalPlaces = 2)
        {
            string numberString = numberToFormat.ToString();
            foreach (NumberSuffix suffix in Enum.GetValues<NumberSuffix>())
            {
                double currentValue = 1 * Math.Pow(10, (int)suffix * 3);
                string? suffixValue = Enum.GetName(typeof(NumberSuffix), (int)suffix);
                if ((int)suffix == 0) { suffixValue = string.Empty; }
                if (numberToFormat >= currentValue)
                {
                    numberString = $"{Math.Round(numberToFormat / currentValue, decimalPlaces, MidpointRounding.ToEven)} {suffixValue}";
                }
            }

            return numberString;
        }
        public void UpdateStatistic(CleanerDataBase? dataBase)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                TotalRemovedBytesProgressBar.Value = 50;
                TotalRemovedLabel.Content = FileBytes.GetFileSizeString(App.auth.statistic.cleaner.releasedBytes);

                TotalRemovedFilesProgressBar.Value = 50;
                TotalFilesLabel.Content = FormatNumber(App.auth.statistic.cleaner.deletedFiles);

                if (dataBase != null)
                {
                    DataBaseBar.Value = 85;
                    DataBaseBarLabel.Content = dataBase.cleaners.Count;
                }
            }));
        }
        private void UpdateUI(int removed, long total, int files)
        {
            double progress = ((double)removed / total) * 100.0;
            ProgressBar.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                ProgressBar.Value = progress;
            }));
            double progressRound = Math.Round(progress);
        }
        private void UpdatePanels(bool reverse = false)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                if (reverse)
                {
                    ButtonPanel.Visibility = System.Windows.Visibility.Visible;
                    ProgressBarPanel.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    ButtonPanel.Visibility = System.Windows.Visibility.Hidden;
                    ProgressBarPanel.Visibility = System.Windows.Visibility.Visible;
                }
            }));
        }
        private const string databasePath = "C:\\Program Files\\WinBooster\\DataBase\\clear.json";
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (File.Exists(databasePath))
            {
                string json = File.ReadAllText(databasePath);
                CleanerDataBase? dataBase = CleanerDataBase.FromJson(json);
                if (dataBase != null)
                {
                    foreach (var script in App.auth.main.scripts.Values)
                    {
                        if (script != null)
                        {
                            script.OnCleanerInit(dataBase);
                        }
                    }
                    Threads.Maximum = dataBase.Count / 2;
                    int removed = 0;
                    int removedFiles = 0;
                    long total = dataBase.Count;
                    UpdatePanels();
                    Task.Factory.StartNew(async() =>
                    {
                        Task<CleanerResult> worker = Task.Run<CleanerResult>(async () =>
                        {
                            List<Task<CleanerResult>> chunkWorkers = new List<Task<CleanerResult>>();
                            double? value = await this.Dispatcher.InvokeAsync(() => Threads.Value);
                            var chunks = CleanerDatabaseUtil.ChunkList<ICleanerWorker>(CleanerDatabaseUtil.GetWorker(dataBase), (int)value);
                            foreach (var chunk in chunks)
                            {
                                Task<CleanerResult> chunkWorker = new Task<CleanerResult>(() =>
                                {
                                    CleanerResult chunkResult = new CleanerResult();
                                    foreach (var item in chunk)
                                    {
                                        try
                                        {
                                            if (CheckCategory(item))
                                            {
                                                var deleted = item.TryDelete();
                                                App.auth.statistic.cleaner.releasedBytes += deleted.bytes;
                                                App.auth.statistic.cleaner.deletedFiles += deleted.files;
                                                chunkResult.bytes += deleted.bytes;
                                                chunkResult.files += deleted.files;
                                                UpdateStatistic(dataBase);
                                                removedFiles += deleted.files;
                                            }
                                        }
                                        catch { }
                                        removed++;
                                        UpdateUI(removed, total, removedFiles);
                                    }
                                    return chunkResult;
                                });
                                chunkWorkers.Add(chunkWorker);
                                chunkWorker.Start();
                            }
                            CleanerResult result = new CleanerResult();
                            foreach (Task<CleanerResult> chunkWorker in chunkWorkers)
                            {
                                CleanerResult chunkWorkerResult = await chunkWorker;
                                result.bytes += chunkWorkerResult.bytes;
                                result.files += chunkWorkerResult.files;
   
                                UpdateStatistic(dataBase);
                            }
                            return result;
                        });
                        CleanerResult workerResult = await worker;
                        foreach (var script in App.auth.main.scripts.Values)
                        {
                            if (script != null)
                            {
                                script.OnCleanerDone(workerResult);
                            }
                        }
                        UpdatePanels(true);
                        long size = workerResult.bytes;
                        long files = workerResult.files;
                        GrowlInfo growl = new GrowlInfo
                        {
                            Message = "💾 Space available: " + FileBytes.GetFileSizeString(size) + "\n♻ Deleted files: " + FormatNumber(files),
                            ShowDateTime = true,
                        };
                        Growl.InfoGlobal(growl);
                        App.auth.SaveStatistic();

                        if (clearListForm != null)
                        {
                            await clearListForm.Dispatcher.BeginInvoke(() =>
                            {
                                clearListForm.UpdateList();
                                clearListForm.UpdateList2();
                            });
                        }
                        //clearListForm.UpdateList();
                        //clearListForm.UpdateList2();
                    });
                }
                else
                {
                    ErrorDataBase();
                }
            }
            else
            {
                ErrorDataBase();
            }
        }

        public void ErrorDataBase()
        {
            GrowlInfo growl = new GrowlInfo
            {
                Message = "DataBase not found",
                ShowDateTime = true,
                IconKey = "ErrorGeometry",
                IconBrushKey = "DangerBrush",
                IsCustom = true
            };
            Growl.InfoGlobal(growl);
        }

        public bool CheckCategory(string category)
        {
            if (checkBoxes.ContainsKey(category))
            {
                CheckBox ch = checkBoxes[category];
                bool? isChecked = this.Dispatcher.Invoke(() => ch.IsChecked);
                if (isChecked == true && ClearListForm.enabledSettings.keyValues.ContainsKey(category) && ClearListForm.enabledSettings.keyValues[category])
                {
                    return true;
                }
                else if (isChecked == true && !ClearListForm.enabledSettings.keyValues.ContainsKey(category))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckCategory(ICleanerWorker worker)
        {
            string category = worker.GetCategory();
            return CheckCategory(category);
        }
        private bool first = true;
        private void Window_Activated(object sender, EventArgs e)
        {
            App.UpdateScreenCapture(this);

            if (File.Exists(databasePath))
            {
                if (first)
                {
                    string json = File.ReadAllText(databasePath);
                    CleanerDataBase? dataBase = CleanerDataBase.FromJson(json);
                    UpdateStatistic(dataBase);
                    if (dataBase != null)
                    {
                        Threads.Maximum = dataBase.Count / 2;
                        Threads.Value = dataBase.Count / 4;
                    }
                    first = false;
                }
            }
        }

        private void Button_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            foreach (CheckBox ch in checkBoxes.Values)
            {
                this.Dispatcher.Invoke(() => ch.IsChecked = !ch.IsChecked);
            }
        }
        public ClearListForm clearListForm = null;
        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            if (clearListForm != null)
            {
                clearListForm.Show();
            }
        }
        public void UpdateCheckboxes()
        {
            if (File.Exists(databasePath))
            {
                string json = File.ReadAllText(databasePath);
                CleanerDataBase? dataBase = CleanerDataBase.FromJson(json);
                if (dataBase != null)
                {
                    lock (checkBoxes)
                    {
                        checkBoxes.Clear();
                        foreach (var script in App.auth.main.scripts.Values)
                        {
                            if (script != null)
                            {
                                script.OnCleanerInit(dataBase);
                            }
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            Categories.Children.Clear();
                        });

                        List<string> categories = new List<string>();
                        foreach (var category in dataBase.cleaners.ToArray())
                        {

                            List<ICleanerWorker> workers = category.GetWorkers();
                            foreach (var worker in workers.ToArray())
                            {
                                string category_text = worker.GetCategory();
                                if (!categories.Contains(category_text))
                                    categories.Add(category_text);
                            }
                        }
                        int row = 0;
                        int col = 0;
                        foreach (var category in categories.ToArray())
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                StackPanel stack = new StackPanel();
                                stack.Margin = new System.Windows.Thickness(5);
                                stack.SetValue(Grid.RowProperty, row);
                                stack.SetValue(Grid.ColumnProperty, col);

                                CheckBox check = new CheckBox();

                                TextBlock text = new TextBlock();
                                text.Text = category;
                                check.Content = text;
                                stack.Children.Add(check);
                                col++;
                                if (col == 3)
                                {
                                    col = 0;
                                    var rowDefinition = new RowDefinition();
                                    rowDefinition.Height = GridLength.Auto;
                                    Categories.RowDefinitions.Add(rowDefinition);
                                    row++;
                                }
                                if (!checkBoxes.ContainsKey(category))
                                {
                                    checkBoxes.Add(category, check);
                                    Categories.Children.Add(stack);
                                }
                            });

                        }
                    }

                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.UpdateScreenCapture(this);
            Task.Factory.StartNew(() =>
            {
                UpdateCheckboxes();
            });
        }
    }
}
