using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Exercise4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Student> studentsToWrite = new List<Student>
            {
                new Student { Name = "Жульен", Group = "G1", DateOfBirth = new DateTime(2001, 10, 22), AverageScore = 4.5M },
                new Student { Name = "Боб", Group = "G1", DateOfBirth = new DateTime(1999, 5, 25), AverageScore = 3.3M},
                new Student { Name = "Лилия", Group = "F2", DateOfBirth = new DateTime(1999, 1, 11), AverageScore = 5M},
                new Student { Name = "Роза", Group = "F2", DateOfBirth = new DateTime(1989, 9, 19), AverageScore = 3.7M}
            };

            WriteStudentsToBinFile(studentsToWrite, "students.dat");

            List<Student> studentsToRead = ReadStudentsFromBinFile("students.dat");

            var group1 = new List<Student>();
            var group2 = new List<Student>();

            foreach (Student studentProp in studentsToRead)
            {
                if (studentProp.Group == "G1")
                    group1.Add(studentProp);
                else if (studentProp.Group == "F2")
                    group2.Add(studentProp);

                Console.WriteLine(studentProp.Name + " " + studentProp.Group + " " + studentProp.DateOfBirth + " " + studentProp.AverageScore);
            }

            string path1 = @"C:\Users\Feona\Desktop";
            string subpath = @"C:\Users\Feona\Desktop\Students\G1.txt";
            string subpath2 = @"C:\Users\Feona\Desktop\Students\F2.txt";

            DirectoryInfo dirInfo = new DirectoryInfo(path1);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            CreateFile(group1, subpath);
            CreateFile(group2, subpath2);
        }

        static void WriteStudentsToBinFile(List<Student> students, string fileName)
        {
            using FileStream fs = new FileStream(fileName, FileMode.Create);
            using BinaryWriter bw = new BinaryWriter(fs);

            foreach (Student student in students)
            {
                bw.Write(student.Name);
                bw.Write(student.Group);
                bw.Write(student.DateOfBirth.ToBinary());
                bw.Write(student.AverageScore);
            }
            bw.Flush();
            bw.Close();
            fs.Close();
        }



        static List<Student> ReadStudentsFromBinFile(string fileName)
        {
            List<Student> result = new();
            using FileStream fs = new FileStream(fileName, FileMode.Open);
            using StreamReader sr = new StreamReader(fs);

            Console.WriteLine(sr.ReadToEnd());

            fs.Position = 0;

            BinaryReader br = new BinaryReader(fs);

            while (fs.Position < fs.Length)
            {
                Student student = new Student();
                student.Name = br.ReadString();
                student.Group = br.ReadString();
                long dt = br.ReadInt64();
                student.DateOfBirth = DateTime.FromBinary(dt);
                student.AverageScore = br.ReadDecimal();

                result.Add(student);
            }

            fs.Close();
            return result;
        }


        static void CreateFile(List<Student> students, string path)
        {
            using FileStream fs = new FileStream(path, FileMode.Create);
            using BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8, false);

            foreach (Student student in students)
            {
                bw.Write(student.Name.ToString());
                bw.Write(student.Group.ToString());
                bw.Write(student.DateOfBirth.ToString());
                bw.Write(student.AverageScore.ToString());

                bw.Write("\n");
            }
            bw.Flush();
            bw.Close();
            fs.Close();

        }
    }
}