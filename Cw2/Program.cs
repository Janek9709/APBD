using System;
using System.Collections.Generic;
using System.IO;

namespace Cw2
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = @"P:\FTP(Public)\pgago\APBD\Polish\Zajęcia 2\dane.csv";
            var hashSet = new HashSet<Student>(new OwnComparator());
            System.IO.StreamWriter error = new System.IO.StreamWriter(@"Z:\APBD\APBD\Cw2\log.txt", false);
            if (!File.Exists(path))
            {
                string text = "File not found, wrong path";
                error.WriteLine(text);
                //nie wiem czy potrzeba tu try
                try
                {
                    throw new FileNotFoundException("Plik nie istnieje");
                }
                catch (FileNotFoundException ex)
                {
                    // Write error.
                    Console.WriteLine(ex);
                }
            }

            using (var stream = new StreamReader(File.OpenRead(path)))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] student = line.Split(',');
                    bool invalid = false;

                    foreach (string text in student)
                    {
                        if (string.IsNullOrEmpty(text))
                            invalid = true;
                    }

                    if (student.Length != 9 || invalid)
                    {

                        foreach (var text in student)
                        {
                            error.Write(text);
                        }
                        error.WriteLine();

                        invalid = false;
                    }
                    else
                    {

                        var studentTmp = new Student
                        {
                            imie = student[0],
                            nazwisko = student[1],
                            kierunek = student[2],
                            tryb = student[3],
                            numer = Int32.Parse(student[4]),
                            data = DateTime.Parse(student[5]),
                            mail = student[6],
                            imieMatki = student[7],
                            imieOjca = student[8]

                        };

                        if (!hashSet.Add(studentTmp))
                        {
                            foreach (var text in student)
                            {
                                error.Write(text);
                            }
                            error.WriteLine();
                        }
                    }
                }
            }

            //tu dopisz by zapisywalo do xmla
        }
    }

    [Serializable]
    internal class Student
    {
        [XmlElement(ElementName = "Imie")]
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public string kierunek { get; set; }
        public string tryb { get; set; }
        public int numer { get; set; }
        public DateTime data { get; set; }
        public string mail { get; set; }
        public string imieMatki { get; set; }
        public string imieOjca { get; set; }

    }

    internal class OwnComparator : IEqualityComparer<Student>
    {
        public bool Equals(Student x, Student y)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals($"{x.imie} {x.nazwisko} {x.numer}",
                $"{y.imie} {y.nazwisko} {y.numer}");
        }

        public int GetHashCode(Student obj)
        {
            return StringComparer.CurrentCultureIgnoreCase.GetHashCode($"{obj.imie} {obj.nazwisko} {obj.numer}");
        }
    }
}
