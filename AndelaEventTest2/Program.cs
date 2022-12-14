using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static TicketsConsole.Program;

/*

Let's say we're running a small entertainment business as a start-up. This means we're selling tickets to live events on a website. An email campaign service is what we are going to make here. We're building a marketing engine that will send notifications (emails, text messages) directly to the client and we'll add more features as we go.

Please, instead of debuging with breakpoints, debug with "Console.Writeline();" for each task because the Interview will be in Coderpad and in that platform you cant do Breakpoints.

*/

namespace TicketsConsole
{
   
    internal class Program
    {
        public class MarketingEngine
        {
            public readonly List<Event> _events;
            public MarketingEngine(List<Event> events)
            {
                _events = events;
            }

            public void SendCustomerNotifications(Customer customer, Event e, bool showPrice = false)
            {
                var price = showPrice ? $" for {e.Price.ToString()}" : string.Empty;
                Console.WriteLine($"{customer.Name} from {customer.City} event {e.Name} at {e.Date} {price}");
            }
        }

        public class Converters
        {
            public static int AlphebiticalDistance(string s, string t)
            {
                try
                {
                    var result = 0;
                    var i = 0;
                    for(i = 0; i < Math.Min(s.Length, t.Length); i++)
                    {
                        // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                        result += Math.Abs(s[i] - t[i]);
                    }
                    for(; i < Math.Max(s.Length, t.Length); i++)
                    {
                        // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                        result += s.Length > t.Length ? s[i] : t[i];
                    }
                    
                    
                    
                    return result;
                }
                catch (WebException e)
                {
                    Console.WriteLine(e);
                    return int.MaxValue;
                }
                catch (TimeoutException e)
                {
                    Console.WriteLine(e);
                    return int.MaxValue;
                }
            } 
        }

        private static Dictionary<Tuple<string, string>, double> StaticCityDistances { get; set; }
        
        static void Main(string[] args)
        {

            var events = new List<Event>{
                new Event(1, "Phantom of the Opera", "New York", new DateTime(2023,12,23),50),
                new Event(2, "Metallica", "Los Angeles", new DateTime(2023,12,02),50),
                new Event(3, "Metallica", "New York", new DateTime(2023,12,06),40),
                new Event(4, "Metallica", "Boston", new DateTime(2023,10,23),50),
                new Event(5, "LadyGaGa", "New York", new DateTime(2023,09,20),90),
                new Event(6, "LadyGaGa", "Boston", new DateTime(2023,08,01),50),
                new Event(7, "LadyGaGa", "Chicago", new DateTime(2023,07,04),50),
                new Event(8, "LadyGaGa", "San Francisco", new DateTime(2023,07,07),10),
                new Event(9, "LadyGaGa", "Washington", new DateTime(2023,05,22),20),
                new Event(10, "Metallica", "Chicago", new DateTime(2023,01,01),50),
                new Event(11, "Phantom of the Opera", "San Francisco", new DateTime(2023,07,04),30),
                new Event(12, "Phantom of the Opera", "Chicago", new DateTime(2024,05,15),60)
            };

            var customer = new Customer()
            {
                Id = 1,
                Name = "John",
                City = "New York",
                BirthDate = new DateTime(1995, 05, 10)
            };

            //Task 1. Same City
            Console.WriteLine("Task 1: Same City Events");

            var sameCityEvents = events.Where(d => d.City == customer.City).ToList();
            MarketingEngine engine = new MarketingEngine(sameCityEvents);
            foreach (var @event in sameCityEvents)
            {
                engine.SendCustomerNotifications(customer, @event);
            }
            
            Console.WriteLine("Task 2:  Near Birthday Events");
            //Task 2 Birthday
            //Ignore all events that have passed and only pick events in this year first. 
            var nextBirthday = new DateTime(DateTime.Now.Year+1, customer.BirthDate.Month, customer.BirthDate.Day);
            
            var birthdayEvents = events.Where(d=>d.Date >= nextBirthday).OrderBy(d => Math.Abs(customer.BirthDate.DayOfYear - d.Date.DayOfYear))
                .Take(10).OrderBy(d=>d.Date).ToList();

            engine = new MarketingEngine(birthdayEvents);
            
            foreach (var @event in birthdayEvents)
            {
                engine.SendCustomerNotifications(customer, @event);
            }
            
            
            //Task 3 Closest Events
            Console.WriteLine("Task 3: Closest Events");
            //Calculate Distances for all cities. 
            //Get all unique cities
            var cities = events.Select(d => d.City).ToList();
            cities.Add(customer.City);
            cities = cities.Distinct().ToList();
            StaticCityDistances = new Dictionary<Tuple<string, string>, double>();
            //Get all distances. Take advantage of cached ones.
            foreach (var city in cities)
            {
                if (!StaticCityDistances.Any(d=> (d.Key.Item1 == city && d.Key.Item2 == customer.City) || (d.Key.Item1 == customer.City && d.Key.Item2 == city)  ))
                {
                    StaticCityDistances.Add(new Tuple<string, string>(city, customer.City), Converters.AlphebiticalDistance(city, customer.City));
                }
            }

            var closeCityEvents = events
                .OrderBy(d => StaticCityDistances.FirstOrDefault(e => e.Key.Item1 == d.City).Value).Take(5).ToList();
            engine = new MarketingEngine(closeCityEvents);
            
            foreach (var @event in closeCityEvents)
            {
                engine.SendCustomerNotifications(customer, @event);
            }
            
            
            //Task 4: Filter by Price
            //Price added to the Event model and constructor. 
            //Random values added to the events model. 
            Console.WriteLine("Task 4: Price Filter Events");
            var priceEvents = events.OrderBy(d => d.Price).Take(5).ToList();
            engine = new MarketingEngine(priceEvents);
            
            foreach (var @event in priceEvents)
            {
                engine.SendCustomerNotifications(customer, @event, true);
            }
            
        }

        public class Event
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public DateTime Date { get; set; }
            public double Price { get; set; }

            public Event(int id, string name, string city, DateTime date, double price)
            {
                this.Id = id;
                this.Name = name;
                this.City = city;
                this.Date = date;
                this.Price = price;
            }
        }

        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public DateTime BirthDate { get; set; }
        }

       
        /*-------------------------------------
        Coordinates are roughly to scale with miles in the USA
           2000 +----------------------+  
                |                      |  
                |                      |  
             Y  |                      |  
                |                      |  
                |                      |  
                |                      |  
                |                      |  
             0  +----------------------+  
                0          X          4000
        ---------------------------------------*/

    }
}

