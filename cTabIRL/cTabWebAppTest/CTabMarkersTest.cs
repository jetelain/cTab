using cTabWebApp;

namespace cTabWebAppTest
{
    public class CTabMarkersTest
    {

        [Fact]
        public void GetMilSymbol_Sidc()
        {
            Assert.Equal("10032500001305000000", CTabMarkers.GetMilSymbol("\\cTab\\img\\mil\\10032500001305000000.paa", ""));
            Assert.Equal("10032500001301000000", CTabMarkers.GetMilSymbol("\\cTab\\img\\mil\\10032500001301000000.paa", ""));
        }

        [Fact]
        public void GetMilSymbol_Legacy()
        {
            Assert.Equal("10031000001211020051", CTabMarkers.GetMilSymbol("\\cTab\\img\\b_mech_inf_wheeled.paa", ""));
            Assert.Equal("10031000111211020051", CTabMarkers.GetMilSymbol("\\cTab\\img\\b_mech_inf_wheeled.paa", "\\A3\\ui_f\\data\\map\\markers\\nato\\group_0.paa"));
            Assert.Equal("10031000121211020051", CTabMarkers.GetMilSymbol("\\cTab\\img\\b_mech_inf_wheeled.paa", "\\A3\\ui_f\\data\\map\\markers\\nato\\group_1.paa"));
            Assert.Equal("10031000131211020051", CTabMarkers.GetMilSymbol("\\cTab\\img\\b_mech_inf_wheeled.paa", "\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa"));
            Assert.Equal("10031000141211020051", CTabMarkers.GetMilSymbol("\\cTab\\img\\b_mech_inf_wheeled.paa", "\\A3\\ui_f\\data\\map\\markers\\nato\\group_3.paa"));
        }
    }
}
