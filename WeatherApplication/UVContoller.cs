using System;
using System.Threading.Tasks;
using WeatherApplication;

namespace WeatherApplication
{
    public class UVController
    {
        private readonly UVModel m_UVModel;
        private readonly UVView m_UVView;
        private UVModel.UVData? m_UVData;

        public UVController(UVModel model,UVView view)
        {
            m_UVModel = model ?? throw new ArgumentNullException(nameof(model));
            m_UVView = view ?? throw new ArgumentNullException(nameof(view));
        }

        public async Task RefreshUVData(double lon, double lat)
        {
            try
            {
                // Call GetUVDataAsync method to retrieve UV data
                m_UVData = await m_UVModel.GetUVDataAsync(lon,lat);
                // Render the UV data
                m_UVView.Render(m_UVData);
            }
            catch (CustomException ex)
            {
                ErrorLogger.Instance.LogError($"An error occurred while retrieving UV data: {ex.Message}");
            }
        }
    }
}
