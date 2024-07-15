using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    internal class NasaDataController
    {
        private readonly NasaDataModel _model;
        private readonly NasaDataView _view;

        public NasaDataController(NasaDataModel model, NasaDataView view)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public async Task FetchAndDisplayImageAsync(double lon, double lat, DateTime date)
        {
            try
            {
                byte[] imageData = await _model.GetImageAsync(lon, lat, date);
                _view.DisplayImage(imageData);
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
