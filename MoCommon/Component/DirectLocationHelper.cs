using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using MoCommon.EntityModel;
using Newtonsoft.Json;
using RestSharp;

namespace MoCommon.Component
{
  public  class DirectLocationHelper
    {
      public delegate void LocationHandler(object responseData, Exception se);
      public event LocationHandler GetLocationComplated;

      public async void GetCurrentLocationInfo()
      {
          Geolocator geolocator = new Geolocator();
          geolocator.DesiredAccuracyInMeters = 50;
          LocationInfo locationInfo = null;
          try
          {
              Geoposition position = await geolocator.GetGeopositionAsync(
                  maximumAge: TimeSpan.FromMinutes(5),
                  timeout: TimeSpan.FromSeconds(30)
                  );

              if (locationInfo == null)
                  locationInfo = new LocationInfo() { Latitude = position.Coordinate.Latitude, Longitude = position.Coordinate.Longitude };

              if (GetLocationComplated != null)
                  GetLocationComplated(locationInfo, null);

          }
          catch (Exception se)
          {
              if (GetLocationComplated != null)
                  GetLocationComplated(null, se);
          }
      }

      public readonly string _requestUrl = "http://loc.desktop.maps.svc.ovi.com/geocoder/rgc/1.0";
      public List<KeyValuePair<string, object>> _postArgumentList = null;
      public event LocationHandler GetLocationCityComplated;

      public string GetRequestUrl(string requestUrl,double latitude,double longitude,ResponseType responseType)
      {
          if (_postArgumentList == null)
              _postArgumentList = new List<KeyValuePair<string, object>>();

          _postArgumentList.Add(new KeyValuePair<string,object>("lat",latitude));
          _postArgumentList.Add(new KeyValuePair<string, object>("long", longitude));
          _postArgumentList.Add(new KeyValuePair<string, object>("output", responseType.ToString().ToLower()));

          return requestUrl;
      }

      public void GetLocationCityInfo(double latitude, double longitude)
      {
          string requestUrl = GetRequestUrl(this._requestUrl, latitude, longitude, ResponseType.Json);
          DataRequestHelper dataRequestHelper = new DataRequestHelper();
          dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.GET, _postArgumentList);
          dataRequestHelper.AsyncResponseComplated += (ResponseData, ex) => 
          {
              if (!string.IsNullOrEmpty(ResponseData.ToString()))
              {
                  LocationCityRepInfo cityRepInfo=JsonConvert.DeserializeObject<LocationCityRepInfo>(ResponseData.ToString());
                  if (GetLocationCityComplated != null)
                      GetLocationCityComplated(cityRepInfo, null);
              }
          };
      }
    
    }

    public enum ResponseType
    {
        Json,
        Xml
    }


}
