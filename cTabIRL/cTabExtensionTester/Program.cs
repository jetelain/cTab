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
            Extension.RvExtensionArgs(sb, int.MaxValue, "Connect", new [] { "\"http://localhost:5000/hub\"", "\"76561198081226363\"", "\"GrueArbre\"", "\"584642\"" }, 4);
            Extension.RvExtensionArgs(sb, int.MaxValue, "StartMission", new[] { "\"Malden\"", "12800", "[2035,6,24,12,0]" }, 3);

            Extension.RvExtensionArgs(sb, int.MaxValue, "UpdateMessages", new[] { "[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",0,\"m1\"]" }, 0);
            Extension.RvExtensionArgs(sb, int.MaxValue, "UpdateMessages", new[] { "[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",0,\"m1\"]", "[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",2,\"m2\"]" }, 0);

            //        //[["_USER_DEFINED #0/4/3",[6184.42,9024.04,0],"hd_dot","ICON",[1,1],0,"Solid","ColorBlack","HELLO",1],
            //["_USER_DEFINED #0/5/3",[6726.9,9003.73,0],"hd_dot","ICON",[1,1],0,"Solid","ColorBlack","WORLD",1]
            //["_USER_DEFINED #0/3/3",[5855.28,8989.5,5841.06,8993.57,5828.87,9005.76,5814.65,9028.11,5798.39,9066.71,5782.14,9115.47,5771.98,9204.87,5769.95,9304.42,5786.2,9416.17,5826.84,9511.66,5885.76,9599.03,5964.99,9682.33,6092.99,9751.41,6231.15,9775.79,6371.34,9767.66,6527.79,9720.93,6643.6,9647.79,6718.77,9527.92,6735.03,9403.98,6710.65,9286.14,6661.88,9233.31,6592.8,9204.87,6493.25,9198.77,6385.57,9221.12,6190.52,9318.65,6123.47,9385.7,6111.28,9434.46,6129.57,9475.09,6178.33,9499.47,6239.28,9491.35,6286.01,9458.84,6298.2,9420.23,6296.17,9379.6,6290.07,9365.38,6286.01,9363.35],"Solid","ColorBlack"]]



            while (true)
            {
                Extension.RvExtensionArgs(sb, int.MaxValue, "UpdateMarkers", new[] { "[\"v\",\"o2\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa\",\"\",\"Bison 01\",\"\",[6076.06,9287.43,178.459],359.76]", "[\"v\",\"o3\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa\",\"\",\"Bison 02\",\"\",[6087.6,9312.65,178.051],359.903]", "[\"v\",\"o5\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP03\",\"\",[6133.26,9267.66,170.838],0.0104469]", "[\"v\",\"o7\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa\",\"\\cTab\\img\\icon_air_contact_ca.paa\",\"Griffon 02\",\"\",[6120.25,9294.33,175.916],0.0432751]", "[\"v\",\"o8\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa\",\"\\cTab\\img\\icon_air_contact_ca.paa\",\"Griffon 01\",\"\",[6136.56,9317.35,176.434],0.00197615]", "[\"v\",\"o4\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP02\",\"\",[6196.91,9289.84,175.783],359.901]", "[\"v\",\"o6\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP04\",\"\",[6196.98,9255.88,168.648],359.997]", "[\"g\",\"o9\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP02\",\"\",[6179.84,9297.33,175.754],0,\"\"]", "[\"g\",\"o10\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP03\",\"\",[6133.7,9265.86,173.151],0,\"o5\"]", "[\"g\",\"o11\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP04\",\"\",[6177.97,9262.54,169.726],0,\"\"]", "[\"u\",\"m0\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:00\",\"\",[5198,9157],400]", "[\"u\",\"m1\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_mech_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:00\",\"\",[5273,9159],400]", "[\"u\",\"m2\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_motor_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:00\",\"\",[5352,9157],400]", "[\"u\",\"m3\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_armor.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:00\",\"\",[5426,9158],400]", "[\"u\",\"m4\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_air.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_1.paa\",\"12:00\",\"\",[5497,9162],400]", "[\"u\",\"m5\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_air.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:01\",\"\",[5578,9157],400]", "[\"u\",\"m6\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_plane.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:01\",\"\",[5648,9157],400]", "[\"u\",\"m7\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_unknown.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_1.paa\",\"12:01\",\"\",[5725,9154],180]", "[\"u\",\"m8\",\"\\cTab\\img\\o_inf_rifle.paa\",\"\",\"12:01\",\"\",[5194,9084],45]", "[\"u\",\"m9\",\"\\cTab\\img\\o_inf_mg.paa\",\"\",\"12:01\",\"\",[5271,9078],400]", "[\"u\",\"m10\",\"\\cTab\\img\\o_inf_at.paa\",\"\",\"12:02\",\"\",[5352,9081],400]", "[\"u\",\"m11\",\"\\cTab\\img\\o_inf_mmg.paa\",\"\",\"12:02\",\"\",[5434,9075],400]", "[\"u\",\"m12\",\"\\cTab\\img\\o_inf_mat.paa\",\"\",\"12:02\",\"\",[5503,9080],400]", "[\"u\",\"m13\",\"\\cTab\\img\\o_inf_aa.paa\",\"\",\"12:02\",\"\",[5582,9073],400]", "[\"u\",\"m14\",\"\\cTab\\img\\o_inf_mmortar.paa\",\"\",\"12:02\",\"\",[5654,9078],400]", "[\"u\",\"m15\",\"\\A3\\ui_f\\data\\map\\markers\\military\\join_CA.paa\",\"\",\"12:02\",\"\",[5189,9009],400]", "[\"u\",\"m16\",\"\\A3\\ui_f\\data\\map\\markers\\military\\circle_CA.paa\",\"\",\"12:02\",\"\",[5278,9004],400]", "[\"u\",\"m17\",\"\\A3\\ui_f\\data\\map\\mapcontrol\\Hospital_CA.paa\",\"\",\"12:02\",\"\",[5348,9005],400]", "[\"u\",\"m18\",\"\\A3\\ui_f\\data\\map\\markers\\military\\warning_CA.paa\",\"\",\"12:02\",\"\",[5426,9004],400]", "[\"u\",\"m19\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_hq.paa\",\"\",\"12:03\",\"\",[5178,8930],400]", "[\"u\",\"m20\",\"\\A3\\ui_f\\data\\map\\markers\\military\\end_CA.paa\",\"\",\"12:03\",\"\",[5266,8936],400]" }, 3);
                Extension.RvExtensionArgs(sb, int.MaxValue, "UpdateMapMarkers", new[] {
                "[[\"_USER_DEFINED #0/4/3\",[6184.42,9024.04,0],\"hd_dot\",\"ICON\",[1,1],0,\"Solid\",\"ColorBlack\",\"HELLO\",1],[\"_USER_DEFINED #0/5/3\",[6726.9,9003.73,0],\"hd_dot\",\"ICON\",[1,1],0,\"Solid\",\"ColorBlack\",\"WORLD\",1]]",
                "[[\"_USER_DEFINED #0/3/3\",[5855.28,8989.5,5841.06,8993.57,5828.87,9005.76,5814.65,9028.11,5798.39,9066.71,5782.14,9115.47,5771.98,9204.87,5769.95,9304.42,5786.2,9416.17,5826.84,9511.66,5885.76,9599.03,5964.99,9682.33,6092.99,9751.41,6231.15,9775.79,6371.34,9767.66,6527.79,9720.93,6643.6,9647.79,6718.77,9527.92,6735.03,9403.98,6710.65,9286.14,6661.88,9233.31,6592.8,9204.87,6493.25,9198.77,6385.57,9221.12,6190.52,9318.65,6123.47,9385.7,6111.28,9434.46,6129.57,9475.09,6178.33,9499.47,6239.28,9491.35,6286.01,9458.84,6298.2,9420.23,6296.17,9379.6,6290.07,9365.38,6286.01,9363.35],\"Solid\",\"ColorBlack\",1]]"
                }, 3);

                for (int i = 1; i < 10; ++i)
                {
                    Thread.Sleep(250);
                    Extension.RvExtensionArgs(sb, int.MaxValue, "UpdatePosition", new[] { "6093", "9256.31", "171.876", "351.846", "[2035,6,24,12,0]", "\"id1\"" }, 5);
                }
                Extension.RvExtensionArgs(sb, int.MaxValue, "UpdateMessages", new[] { "[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",1,\"m1\"]", "[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",2,\"m2\"]", "[\"12:01 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"EH BEH\",0,\"m3\"]", "[\"12:01 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"EH BEH\",2,\"m4\"]", "[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"test\",0,\"m5\"]", "[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"test\",2,\"m6\"]" }, 0);

            }

            //Line 679: 


            /*"UpdateMessages",new[]{"[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",1,\"m1\"]","[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",2,\"m2\"]","[\"12:01 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"EH BEH\",0,\"m3\"]","[\"12:01 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"EH BEH\",2,\"m4\"]","[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"test\",0,\"m5\"]","[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"test\",2,\"m6\"]","[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Hello world !\",0,\"m7\"]"}
"UpdateMessages",new[]{"[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",1,\"m1\"]","[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",2,\"m2\"]","[\"12:01 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"EH BEH\",0,\"m3\"]","[\"12:01 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"EH BEH\",2,\"m4\"]","[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"test\",0,\"m5\"]","[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"test\",2,\"m6\"]","[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Hello world !\",0,\"m7\"]","[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Hello world !\",2,\"m8\"]"}*/
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
