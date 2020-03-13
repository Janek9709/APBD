﻿using Newtonsoft.Json;
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
            int argsLengthTmp = args.Length;
            string input = "";
            string input2 = "";
            string input3 = "";
            //Console.WriteLine(argsLengthTmp);
            if (argsLengthTmp == 1)
            {
                input = args[0];
            }
            else if(argsLengthTmp == 2)
            {
                 input = args[0];
                 input2 = args[1];
            }
            else if(argsLengthTmp == 3)
            {
                input = args[0];
                input2 = args[1];
                input3 = args[2];
            }

            //string projectDirectory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName).FullName;
            //string path = @projectDirectory + "/DaneDoWczytania/dane.csv";
            //input = input.Length == 0 ? @projectDirectory + "/DaneDoWczytania/dane.csv" : input;
            input = input.Length == 0 ? "data.csv" : input;
            //input2 = input2.Length == 0 ? @projectDirectory + "/result.xml" : input2;
            input2 = input2.Length == 0 ? "result.xml" : input2;

            input3 = input3.Length == 0 ? "xml" : input3;

            //Console.WriteLine(input + " " + input2 + " " + input3);

            string path = input;

            var hashSet = new HashSet<Student>(new OwnComparator());
            //System.IO.StreamWriter error = new System.IO.StreamWriter(@projectDirectory + "/log.txt", false);
            System.IO.StreamWriter error = new System.IO.StreamWriter("log.txt", false);
            
            if (!File.Exists(path))
            {
                string text = "File not found";
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
            //!Path.IsPathRooted(path) ||
            else if (!Path.HasExtension(path) || (input3 != "xml" && input3 != "json") || !input2.Contains(input3))
            {
                string text = "Wrong path";
                error.WriteLine(text);
                try
                {
                    throw new ArgumentException("Bledna sciezka lub zle/brak podane rozszerzenie, lub brak drugiego argumentu, przez co jest sciezka domyslna do xmla, a rozszerzenie json");
                }
                catch (ArgumentException ex)
                {
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
                            error.Write(text+" ");
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
                            numer = (student[4]),
                            data = DateTime.Parse(student[5]).ToShortDateString(),
                            mail = student[6],
                            imieMatki = student[7],
                            imieOjca = student[8]

                        };

                        if (!hashSet.Add(studentTmp))
                        {
                            foreach (var text in student)
                            {
                                error.Write(text+" ");
                            }
                            error.WriteLine();
                        }
                    }
                }
            }


            //XmlElementAttribute attr =  new XmlElementAttribute("Alumni", typeof(Graduate));

            //XmlSerializer serializer = new XmlSerializer(typeof(HashSet<Student>), root);


            var hashSetActive = new HashSet<ActiveStudies>(new ActiveOwnComparator());

            var hashSetActiveFinalTrue = new HashSet<ActiveStudies>(new ActiveOwnComparator());
            //logika dodawania do tego hashsetu
            var numberOfComputerScience = 0;

            foreach (Student tmp in hashSet)
            {
                var activeStudentTmp = new ActiveStudies
                {
                    name = tmp.studies.kierunek,
                    numberOfStudents = numberOfComputerScience
                };

                hashSetActive.Add(activeStudentTmp);
            }

            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            foreach (var tmp2 in hashSetActive)
            {
                dictionary.Add(tmp2.name, 0);
            }

            foreach (var tmp in hashSet)
            {
                foreach (var tmp2 in hashSetActive)
                {
                    if (tmp.studies.kierunek == tmp2.name && dictionary.ContainsKey(tmp.studies.kierunek))
                    {
                        dictionary[tmp.studies.kierunek] += 1;
                    }
                }
            }

            foreach (KeyValuePair<string, int> kvp in dictionary)
            {
                var activeStudentTmp = new ActiveStudies
                {
                    name = kvp.Key,
                    numberOfStudents = kvp.Value
                };

                hashSetActiveFinalTrue.Add(activeStudentTmp);
            }



            var getOverall = new SummaryArray
            {
                createdAt = DateTime.Today.ToString("dd.MM.yyyy"),
                author = "Jan Biniek",
                studenci = hashSet,
                activeStudents = hashSetActiveFinalTrue
            };
            if (input3 == "xml")
            {
                //tu wywolanie calego xmla
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                FileStream writer = new FileStream(input2, FileMode.Create);
                XmlRootAttribute root = new XmlRootAttribute("uczelnia");
                XmlSerializer serializer = new XmlSerializer(typeof(SummaryArray), root);

                serializer.Serialize(writer, getOverall, ns);
                //tu koniec wywolania xmla
            }
            else if (input3 == "json")
            {
                //input2 = @projectDirectory + "/dataJson.json";
                //if (input2.Contains("xml"))
                //{
                //    input2 = "result.json";
                //    Console.WriteLine("Plik json zapisano w domyslnej lokalizacji ");
                //}
                //tu poczatek json
                var uczelnia = new Uczelnia
                {
                    uczelnia = getOverall
                };
                string output = JsonConvert.SerializeObject(uczelnia, Formatting.Indented);
                //Console.WriteLine(output);
                File.WriteAllText(input2, output);
                
                //tu koniec json
            }
        }
    }
}
