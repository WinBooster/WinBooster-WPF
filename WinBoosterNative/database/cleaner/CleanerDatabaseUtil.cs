namespace WinBoosterNative.database.cleaner
{
    public class CleanerDatabaseUtil
    {
        public static List<ICleanerWorker> GetWorker(CleanerCategory dataBase)
        {
            List<ICleanerWorker> temp = new List<ICleanerWorker>();
            if (dataBase.custom != null)
            {
                foreach (var file in dataBase.custom)
                {
                    temp.Add(file);
                }
            }
            foreach (var file in dataBase.listFiles)
            {
                temp.Add(file);
            }
            foreach (var file in dataBase.listFolders)
            {
                temp.Add(file);
            }
            foreach (var file in dataBase.paternFiles)
            {
                temp.Add(file);
            }
            foreach (var file in dataBase.allFilesRecursives)
            {
                temp.Add(file);
            }
            foreach (var file in dataBase.paternFilesRecursives)
            {
                temp.Add(file);
            }
            foreach (var file in dataBase.listFilesIsNotLanguage)
            {
                temp.Add(file);
            }
            foreach (var file in dataBase.filesIsNotLanguageByPatern)
            {
                temp.Add(file);
            }
            foreach (var file in dataBase.foldersIsNotLanguageByPatern)
            {
                temp.Add(file);
            }
            return temp;
        }
        public static List<ICleanerWorker> GetWorker(CleanerDataBase dataBase)
        {
            List<ICleanerWorker> temp = new List<ICleanerWorker>();
            foreach (var cleaner in dataBase.cleaners)
            {
                if (cleaner.custom != null)
                {
                    foreach (var file in cleaner.custom)
                    {
                        temp.Add(file);
                    }
                }
                foreach (var file in cleaner.listFiles)
                {
                    temp.Add(file);
                }
                foreach (var file in cleaner.listFolders)
                {
                    temp.Add(file);
                }
                foreach (var file in cleaner.paternFiles)
                {
                    temp.Add(file);
                }
                foreach (var file in cleaner.allFilesRecursives)
                {
                    temp.Add(file);
                }
                foreach (var file in cleaner.paternFilesRecursives)
                {
                    temp.Add(file);
                }
                foreach (var file in cleaner.listFilesIsNotLanguage)
                {
                    temp.Add(file);
                }
                foreach (var file in cleaner.filesIsNotLanguageByPatern)
                {
                    temp.Add(file);
                }
                foreach (var file in cleaner.foldersIsNotLanguageByPatern)
                {
                    temp.Add(file);
                }
            }
             return temp;
        }

        public static List<List<T>> ChunkList<T>(List<T> originalList, int chunkSize)
        {
            List<List<T>> chunks = new List<List<T>>();

            for (int i = 0; i < originalList.Count; i += chunkSize)
            {
                chunks.Add(originalList.GetRange(i, Math.Min(chunkSize, originalList.Count - i)));
            }

            return chunks;
        }
    }
}
