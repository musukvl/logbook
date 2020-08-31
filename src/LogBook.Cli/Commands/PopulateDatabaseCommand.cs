using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogBook.Database;
using LogBook.Database.Model;
using McMaster.Extensions.CommandLineUtils;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace LogBook.Cli.Commands
{
    [Command("populate-db", Description = "Populate database from Json files")]
    public class PopulateDatabaseCommand 
    {
        private readonly LogBookDbContext _dbContext;

        [Option("-wf|--waypoints-file", "Waypoints json file", CommandOptionType.SingleValue)]
        [Required]
        [FileExists]
        public string WayPointFile { get; set; }
        
        [Option("-rf|--routes-file", "Routs json file", CommandOptionType.SingleValue)]
        [FileExists]
        [Required]
        public string RoutsFile { get; set; }

        public PopulateDatabaseCommand(LogBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        protected async Task<int> OnExecute()
        {
            var users = new Dictionary<string, User>();
            var routes = new Dictionary<string, Route>();
            
            var routeType = new
            {
                _id = "",
                user = ""
            };
            
            foreach (var routeLine in File.ReadLines(RoutsFile))
            {
                if (string.IsNullOrEmpty(routeLine))
                    continue;
                var routeObj = JObject.Parse(routeLine);
                User user;
                var userName = (string)routeObj["user"];
                if (!users.ContainsKey(userName))
                {
                    user = new User
                    {
                        Name =  userName, 
                        Id = Guid.NewGuid(),
                        Routes = new List<Route>()
                    };
                    users.Add(userName, user);
                }
                else
                {
                    user = users[userName];
                }
                
                var route = new Route
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    Name = routeObj["from"] + " -> " + routeObj["to"],
                    UserId = user.Id,
                    WayPoints = new List<WayPoint>()
                };
                routes.Add((string)routeObj["_id"], route);
                user.Routes.Add(route);
            }
            
            
            var waypoints = new List<WayPoint>();
            foreach (var waypointLine in File.ReadLines(WayPointFile))
            {
                if (string.IsNullOrEmpty(waypointLine))
                    continue;
                var waypointObj = JObject.Parse(waypointLine);
                var routeId = (string)waypointObj["trip"];
                var route = routes[routeId];

                var lat = (double)waypointObj["position"][0];
                var lon = (double)waypointObj["position"][1];
                
                var waypoint = new WayPoint
                {
                    Id = Guid.NewGuid(),
                    RouteId = route.Id,
                    Date = UnixTimeStampToDateTime((double)waypointObj["datetime"]),
                    Location = new Point(lat, lon),
                };
                waypoints.Add(waypoint);
                route.WayPoints.Add(waypoint);
            }


            foreach (var user in users)
            {
                await _dbContext.Users.AddAsync(user.Value);
            }
            
            foreach (var route in routes)
            {
                await _dbContext.Routes.AddAsync(route.Value);
            }
            
            foreach (var waypoint in waypoints)
            {
                await _dbContext.WayPoints.AddAsync(waypoint);
            }

            await _dbContext.SaveChangesAsync();
            return 0;
        }
        
        public static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds( unixTimeStamp ).ToLocalTime();
            return dtDateTime;
        }
    }
}
