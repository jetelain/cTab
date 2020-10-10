using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using cTabExtension;

namespace cTabExtensionTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var sb = new StringBuilder();
            Extension.RvExtensionVersion(sb, int.MaxValue);
            Extension.RVExtensionRegisterCallback(Callback);
            Extension.RvExtension(sb, int.MaxValue, "Warmup");
            Extension.RvExtension(sb, int.MaxValue, "Debug");
            Extension.RvExtensionArgs(sb, int.MaxValue, "Connect", new [] { "\"http://localhost:5000/hub\"", "\"PlayerUID\"", "\"Player\"", "\"Key\"" }, 4);
            Extension.RvExtensionArgs(sb, int.MaxValue, "StartMission", new[] { "\"Malden\"", "12800", "[2035,6,24,12,0]" }, 3);

            while (true)
            {
                Extension.RvExtensionArgs(sb, int.MaxValue, "UpdateMarkers", new[] { "[\"veh\",\"id3\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa\",\"\",\"Bison 01\",\"\",[6076.04,9287.31,178.611],0.00514363]", "[\"veh\",\"id4\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa\",\"\",\"Bison 02\",\"\",[6087.61,9312.58,178.044],359.997]", "[\"veh\",\"id6\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP03\",\"\",[6133.26,9267.66,170.852],0.00201088]", "[\"veh\",\"id8\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa\",\"\\cTab\\img\\icon_air_contact_ca.paa\",\"Griffon 02\",\"\",[6120.25,9294.35,175.945],0.0203117]", "[\"veh\",\"id9\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa\",\"\\cTab\\img\\icon_air_contact_ca.paa\",\"Griffon 01\",\"\",[6136.56,9317.35,176.419],0.00425643]", "[\"veh\",\"id5\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP02\",\"\",[6196.92,9289.88,175.817],359.991]", "[\"veh\",\"id7\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP04\",\"\",[6196.98,9255.89,168.663],359.999]", "[\"grp\",\"id10\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP02\",\"\",[6179.84,9297.33,175.751],0,\"id11\"]", "[\"grp\",\"id12\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP03\",\"\",[6133.7,9265.86,173.248],0,\"id6\"]", "[\"grp\",\"id13\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP04\",\"\",[6177.97,9262.54,169.716],0,\"id14\"]" }, 3);

                for (int i = 1; i < 10; ++i)
                {
                    Thread.Sleep(250);
                    Extension.RvExtensionArgs(sb, int.MaxValue, "UpdatePosition", new[] { "6093", "9256.31", "171.876", "351.846", "[2035,6,24,12,0]", "\"id1\"" }, 5);
                }
            }
            /*
"StartMission",new[]{"\"Malden\"","12800","[2035,6,24,12,0]"}
"UpdatePosition",new[]{"6093","9256.31","171.868","0","[2035,6,24,12,0]"}
"UpdateMarkers",new[]{"[\"veh\",\"id1\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa\",\"\",\"Bison 01\",\"\",[6076.04,9287.31,178.626],0]","[\"veh\",\"id2\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa\",\"\",\"Bison 02\",\"\",[6087.61,9312.58,178.062],0]","[\"veh\",\"id3\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP02\",\"\",[6196.93,9289.89,175.799],0]","[\"veh\",\"id4\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP03\",\"\",[6133.26,9267.66,170.821],0]","[\"veh\",\"id5\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP04\",\"\",[6196.98,9255.89,168.631],0]","[\"veh\",\"id6\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa\",\"\\cTab\\img\\icon_air_contact_ca.paa\",\"Griffon 02\",\"\",[6120.25,9294.38,175.901],0]","[\"veh\",\"id7\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa\",\"\\cTab\\img\\icon_air_contact_ca.paa\",\"Griffon 01\",\"\",[6136.56,9317.35,176.324],0]","[\"grp\",\"id8\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP02\",\"\",[6179.84,9297.33,175.751]]","[\"grp\",\"id9\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP03\",\"\",[6133.7,9265.87,173.257]]","[\"grp\",\"id10\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP04\",\"\",[6177.97,9262.54,169.716]]"}
"UpdatePosition",new[]{"6093","9256.31","171.868","0","[2035,6,24,12,0]"}
*/
        }

        static int Callback(string name, string function, string data)
        {
            Console.WriteLine(data);
            return 0;
        }
    }
}
