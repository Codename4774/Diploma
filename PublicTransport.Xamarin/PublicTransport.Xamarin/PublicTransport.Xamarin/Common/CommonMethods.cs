using GTFS.Entities;
using GTFS.Entities.Enumerations;
using PublicTransport.Xamarin.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.Common
{
    public static class CommonMethods
    {
        public static int GetCurrentDay()
        {
            DateTime currentDateTime = DateTime.Now;

            return (int)currentDateTime.DayOfWeek;
        }

        public static int MinutesDiffWithCurrent(TimeOfDay timeOfDay)
        {
            DateTime current = DateTime.Now;

            return (current.Hour * 60 + current.Minute - timeOfDay.Hours * 60 - timeOfDay.Minutes);
        }

        public static string GetRouteTypeStr(int routeType)
        {
            switch (routeType)
            {
                case 0:
                    {
                        return "Tram";
                    }
                    break;
                case 1:
                    {
                        return "Metro";
                    }
                    break;
                case 2:
                    {
                        return "Rail";
                    }
                    break;
                case 3:
                    {
                        return "Bus";
                    }
                    break;
                case 4:
                    {
                        return "Ferry";
                    }
                    break;
                case 5:
                    {
                        return "Cable tram";
                    }
                    break;
                case 6:
                    {
                        return "Aerial lift";
                    }
                    break;
                case 7:
                    {
                        return "Funicular";
                    }
                    break;
                default:
                    {
                        return "Unknown";
                    }
    
                }
            }

        public static ImageSource GetIconForRouteType(int routeType)
        {
            string fileName = "";

            switch (routeType)
            {
                case 0:
                    fileName = Constants.TRAM_ICON_FILE_PATH;
                    break;
                case 1:
                    fileName = Constants.METRO_ICON_FILE_PATH;
                    break;
                case 2:
                    fileName = Constants.RAIL_ICON_FILE_PATH;
                    break;
                case 3:
                    fileName = Constants.BUS_ICON_FILE_PATH;
                    break;
                case 4:
                    fileName = Constants.FERRY_ICON_FILE_PATH;
                    break;
                case 5:
                    fileName = Constants.CABLE_TRAM_ICON_FILE_PATH;
                    break;
                case 6:
                    fileName = Constants.AERIAL_LIFT_ICON_PATH;
                    break;
                case 7:
                    fileName = Constants.FUNICULAR_ICON_FILE_PATH;
                    break;
                default:
                    fileName = Constants.UNKNOWN_ICON_FILE_PATH;
                    break;
            }


            return ServiceProvider.ImageResourceManager.GetImageSourceFromCache(fileName);
        }

    }
}
