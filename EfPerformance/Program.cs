using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EfPerformance
{
    internal class Program
    {
        private static readonly int iterationCount = 50000;

        private static void Main(string[] args)
        {

            Case1();

            Case1_1();

            Case2();

            Case3();

            Case4();

            Case5();

            Console.ReadLine();
        }

        //Add and SaveChanges inside loop
        private static void Case1()
        {
            var sw = Stopwatch.StartNew();

            using (var dbContext = new TestContext())
            {
                for (var i = 0; i < iterationCount; i++)
                {
                    var person = new Person();
                    person.Name = "Person " + i;

                    dbContext.Person.Add(person);

                    dbContext.SaveChanges();
                }
            }

            sw.Stop();

            Console.WriteLine("Case 1 Took:" + sw.ElapsedMilliseconds + "ms");
        }

        //Add and SaveChanges inside loop with new DbContext for every operation
        private static void Case1_1()
        {
            var sw = Stopwatch.StartNew();


            for (var i = 0; i < iterationCount; i++)
            {
                using (var dbContext = new TestContext())
                {

                    var person = new Person();
                    person.Name = "Person " + i;

                    dbContext.Person.Add(person);

                    dbContext.SaveChanges();
                }
            }

            sw.Stop();

            Console.WriteLine("Case 1_1 Took:" + sw.ElapsedMilliseconds + "ms");
        }

        //Add inside loop, SaveChanges outside loop
        private static void Case2()
        {
            var sw = Stopwatch.StartNew();

            using (var dbContext = new TestContext())
            {
                for (var i = 0; i < iterationCount; i++)
                {
                    var person = new Person();
                    person.Name = "Person " + i;

                    //Change tracking started
                    dbContext.Person.Add(person);
                }

                dbContext.SaveChanges();
            }

            sw.Stop();

            Console.WriteLine("Case 2 Took:" + sw.ElapsedMilliseconds + "ms");
        }

        //Add inside loop, SaveChanges outside loop, AutoDetectionChangesEnabled = false
        private static void Case3()
        {
            var sw = Stopwatch.StartNew();

            using (var dbContext = new TestContext())
            {
                dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                for (var i = 0; i < iterationCount; i++)
                {
                    var person = new Person();
                    person.Name = "Person " + i;

                    dbContext.Person.Add(person);
                }

                dbContext.SaveChanges();
               
                dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }

            sw.Stop();

            Console.WriteLine("Case 3 Took:" + sw.ElapsedMilliseconds + "ms");
        }

        //Temporary list inside loop, AddRange at once outside loop, SaveChanges outside loop
        private static void Case4()
        {
            var sw = Stopwatch.StartNew();

            var tempPersons = new List<Person>();

            using (var dbContext = new TestContext())
            {
                for (var i = 0; i < iterationCount; i++)
                {
                    var person = new Person();
                    person.Name = "Person " + i;

                    tempPersons.Add(person);
                }

                dbContext.Person.AddRange(tempPersons);

                dbContext.SaveChanges();
            }

            sw.Stop();

            Console.WriteLine("Case 4 Took:" + sw.ElapsedMilliseconds + "ms");
        }

        //SqlBulkCopy
        private static void Case5()
        {
            var sw = Stopwatch.StartNew();

            var tempPersons = new List<Person>();

            using (var dbContext = new TestContext())
            {
                for (var i = 0; i < iterationCount; i++)
                {
                    var person = new Person();
                    person.Name = "Person " + i;

                    tempPersons.Add(person);
                }

                //Bulk insert at once using SqlBulkCopy
                dbContext.BulkInsert(tempPersons);
            }

            sw.Stop();

            Console.WriteLine("Case 5 Took:" + sw.ElapsedMilliseconds + "ms");
        }

    }
}
