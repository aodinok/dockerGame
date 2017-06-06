using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Security.Cryptography; 

namespace UkrferryDocker
{
    public class RecordItem
    {
        private string lvlName;
        private string recordsMan;
        private int seconds;
        private int record;

        public string LvlName { get { return lvlName; } }
        public int Record { get { return record; } }
        public string RecordsMan { get { return recordsMan; } }
        public int Seconds { get { return seconds; } }

        public RecordItem(string _lvlName, string _recordsMan, int _seconds, int _record)
        {
            lvlName = _lvlName;
            recordsMan = _recordsMan;
            seconds = _seconds;
            record = _record;
        }
    }

    public class Records
    {
        private const string pass = "UkrFerryDockerPass";
        private string recordsFileName = AppDomain.CurrentDomain.BaseDirectory + "\\data\\records.dat";
        private List<RecordItem> recordsLst;
        public List<RecordItem> RecordsLst { get { return recordsLst; } }

        public Records()
        {
            ReadRecordsLst(); // читаем список рекордов
        }

        private static string Encrypt(string clearText, string Password)
        {
            byte[] clearBytes =
              System.Text.Encoding.Unicode.GetBytes(clearText);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            byte[] encryptedData = Encrypt(clearBytes,
                     pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);
        }

        private static string Decrypt(string cipherText, string Password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
            byte[] decryptedData = Decrypt(cipherBytes,
                pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }

        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms,
               alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        } 

        private void ReadRecordsLst()
        {
            recordsLst = new List<RecordItem>();
            // поиск файла с рекордами и загрузка информации из него для отображения рекордов по доступным уровням
            if (File.Exists(recordsFileName))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;
                XmlReader reader = XmlReader.Create(recordsFileName, settings);

                try
                {
                    while (reader.Read())
                        if (reader.IsStartElement("record"))
                        {
                            reader.ReadStartElement("record");

                            reader.ReadStartElement("levelName");
                            string lvlName = Decrypt(reader.ReadContentAsString(), pass);
                            reader.ReadEndElement();

                            reader.ReadStartElement("recordVal");
                            int lvlRecord = Int32.Parse(Decrypt(reader.ReadContentAsString(), pass));
                            reader.ReadEndElement();

                            reader.ReadStartElement("recordSeconds");
                            int lvlSeconds = Int32.Parse(Decrypt(reader.ReadContentAsString(), pass));
                            reader.ReadEndElement();

                            reader.ReadStartElement("recordsMan");
                            string lvlRecordsMan = Decrypt(reader.ReadContentAsString(), pass);
                            reader.ReadEndElement();

                            recordsLst.Add(new RecordItem(lvlName, lvlRecordsMan, lvlSeconds, lvlRecord));
                        }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Файл с рекордами был поврежден!", "Внимание!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    reader.Close();
                    File.Delete(recordsFileName);
                }

                reader.Close();
            }
        }

        public void NewRecord(RecordItem recItem)
        {
            // перед записью списка рекордом обновим список рекордов, возможно он уже изменился
            ReadRecordsLst();
            // пройдемся по списку рекордов, если найдем рекорд изменим его
            foreach (RecordItem actRecItem in recordsLst)
                if (actRecItem.LvlName == recItem.LvlName)
                {
                    // найден рекорд для данного уровня, нужно удалить его
                    recordsLst.Remove(actRecItem);
                    break;
                }
            recordsLst.Add(recItem);
            // производим запись в файл рекордов!
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            using (XmlWriter writer = XmlWriter.Create(recordsFileName, settings))
            {
                writer.WriteStartElement("UkrferryDockerRecords");
                foreach (RecordItem item in recordsLst)
                {
                    writer.WriteStartElement("record");
                    writer.WriteElementString("levelName", Encrypt(item.LvlName, pass));
                    writer.WriteElementString("recordVal", Encrypt(item.Record.ToString(), pass));
                    writer.WriteElementString("recordSeconds", Encrypt(item.Seconds.ToString(), pass));
                    writer.WriteElementString("recordsMan", Encrypt(item.RecordsMan.ToString(), pass));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}
