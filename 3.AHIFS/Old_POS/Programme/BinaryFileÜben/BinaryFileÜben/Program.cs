﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryFileÜben
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteDefaultValues(@"randomNumbers.bin");

            DisplayValues(@"randomNumbers.bin");
        }

        public static void WriteDefaultValues(string fileName)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writer.Write("Hallo ich bin Andi");
            }
        }

        public static void DisplayValues(string fileName)
        {
            string autoSaveTime;

            if (File.Exists(fileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        autoSaveTime = reader.ReadString();
                        Console.WriteLine("Auto save time set to: " + autoSaveTime);
                    }
                }
            }
        }

        public override PersonenManager Load()
        {

            using (BinaryReader br = new BinaryReader(File.Open(Filename, FileMode.Open)))
            {
                byte Personentyp;
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    Personentyp = br.ReadByte();
                    if (Personentyp == 77)
                    {
                        int counterOfMitarbeiter = 2;
                        pm = GetTheData(counterOfMitarbeiter, br, pm, Personentyp);
                    }
                    else if (Personentyp == 86)
                    {
                        int counterOfVorgesetzter = 3;
                        pm = GetTheData(counterOfVorgesetzter, br, pm, Personentyp);
                    }
                }
            }
            return pm;
        }

        public override void Save(PersonenManager pm)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(Filename, FileMode.Create)))
            {
                foreach (Person p in pm.PersonenReadOnly())
                {
                    if (p.PersonenTyp == "M")
                    {
                        WriteByteArrayToStream(bw, p);
                    }
                    else if (p.PersonenTyp == "V")
                    {
                        WriteByteArrayToStream(bw, p);
                        byte[] Data = ConvertStringToByteArray(p.toPraemie());
                        WriteByteArrayWithPrefix(bw, Data);
                    }
                }
            }
        }

        #region Private Methoden
        private byte[] ConvertStringToByteArray(string input)
        {
            byte[] output;
            output = Encoding.ASCII.GetBytes(input);

            return output;
        }

        private void WriteByteArrayWithPrefix(BinaryWriter bw, byte[] data)
        {
            int length = data.Length;
            bw.Write(length);
            bw.Write(data);
        }

        private void WriteByteArrayToStream(BinaryWriter bw, Person p)
        {
            byte[] Data;

            if (p.PersonenTyp == "M")
            {
                Data = ConvertStringToByteArray(p.PersonenTyp);
                WriteByteArrayWithPrefix(bw, Data);
            }

            if (p.PersonenTyp == "V")
            {
                Data = ConvertStringToByteArray(p.PersonenTyp);
                WriteByteArrayWithPrefix(bw, Data);
            }

            Data = ConvertStringToByteArray(p.Name);
            WriteByteArrayWithPrefix(bw, Data);

            Data = ConvertStringToByteArray(p.toMitarbeiternummer());
            WriteByteArrayWithPrefix(bw, Data);
        }

        private PersonenManager GetTheData(int counter, BinaryReader br, PersonenManager pm, byte Personentyp)
        {
            string[] TextData = new string[counter];

            for (int idx = 0; idx < counter; idx++)
            {
                int NumberOfBytes = br.ReadInt32();
                byte[] Data = new byte[NumberOfBytes];

                for (int i = 0; i < NumberOfBytes; i++)
                {
                    Data[i] = br.ReadByte();
                }
                string Text = Encoding.ASCII.GetString(Data);
                TextData[idx] = Text;
            }
            pm = StringToList(TextData, pm, Personentyp);

            return pm;
        }

        private PersonenManager StringToList(string[] personen, PersonenManager pm, byte Personentyp)
        {
            switch (Personentyp)
            {
                case 77:
                    if (personen.Length > 2)
                    {
                        throw new Exception("Ein Mitarbeiter hat keine Prämie!!!");
                    }
                    pm.AddPerson(new Mitarbeiter(personen[0], personen[1]));
                    break;

                case 86:
                    if (personen[2] == "")
                    {
                        throw new Exception("Ein Vorgesetzter hat eine Prämie!!!");
                    }
                    pm.AddPerson(new Vorgesetzter(personen[0], personen[1], Convert.ToDouble(personen[2])));
                    break;
            }

            return pm;
        }

        private string getEncodedString(BinaryReader br)
        {
            return Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
        }
        #endregion
    }
}
