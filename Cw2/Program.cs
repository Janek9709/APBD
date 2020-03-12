using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Cw2
{
    public class Program
    {
        static void Main(string[] args)
        {
            string projectDirectory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName).FullName;
            string path = @projectDirectory + "/DaneDoWczytania/dane.csv";

            var hashSet = new HashSet<Student>(new OwnComparator());

            System.IO.StreamWriter error = new System.IO.StreamWriter(@projectDirectory + "/log.txt", false);

            if (!File.Exists(path))
            {
                string text = "File not found, wrong path";
                error.WriteLine(text);
                try
                {
                    throw new FileNotFoundException("Plik nie istnieje");
                }
                catch (FileNotFoundException ex)
                {
                    // Write error.
                    Console.WriteLine(ex);
                    Environment.Exit(1);
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
                            studies = new Studies
                            {
                                kierunek = student[2],
                                tryb = student[3]
                            },
                            numer = "s"+(student[4]),
                            data = DateTime.Parse(student[5]).ToShortDateString(),
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
            FileStream writer = new FileStream(@projectDirectory+"/data.xml", FileMode.Create);
            XmlRootAttribute root = new XmlRootAttribute("uczelnia");
            //XmlElementAttribute attr =  new XmlElementAttribute("Alumni", typeof(Graduate));

            //XmlSerializer serializer = new XmlSerializer(typeof(HashSet<Student>), root);
            XmlSerializer serializer = new XmlSerializer(typeof(SummaryArray), root);

            var hashSetActive = new HashSet<ActiveStudies>();
            //logika dodawania do tego hashsetu
            var numberOfComputerScience = 0;
            var numberOfNewMediaArt = 0;
            foreach (Student tmp in hashSet)
            {
                if (tmp.studies.kierunek.Contains("Informatyka"))
                {
                    numberOfComputerScience++;
                }
                else
                {
                    numberOfNewMediaArt++;
                }
            }

            var activeStudentTmp = new ActiveStudies
            {
                name = "Computer Science",
                numberOfStudents = numberOfComputerScience
            };

            var activeStudentTmp2 = new ActiveStudies
            {
                name = "New Media Art",
                numberOfStudents = numberOfNewMediaArt
            };


            hashSetActive.Add(activeStudentTmp);
            hashSetActive.Add(activeStudentTmp2);

            var getOverall = new SummaryArray
            {
                createdAt = DateTime.Today.ToString("dd-MM-yyyy"),
                author = "Jan Biniek",
                studenci = hashSet,
                activeStudents = hashSetActive
            };

            //serializer.Serialize(writer, hashSet);
            serializer.Serialize(writer, getOverall);
        }
    }
}
