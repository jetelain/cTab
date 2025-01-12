using System.Text;
using cTabExtension;

namespace cTabExtensionTester
{
    internal class Program
    {
        private static bool loop = true;
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("** Connect **");
                Extension.RvExtensionArgsImpl("Warmup", new string[0]);
                Extension.RvExtensionArgsImpl("Debug", new string[0]);
                Extension.RvExtensionArgsImpl("Connect", new[] { "\"http://localhost:5000/hub\"", "\"76561198081226363\"", "\"GrueArbre\"", "\"584642\"" });
                Extension.RvExtensionArgsImpl("StartMission", new[] { "\"Malden\"", "12800", "[2035,6,24,12,0]", "\"3.0\"", "\"3.0\"" });

                Extension.RvExtensionArgsImpl("UpdateMessages", new[] { "[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",0,\"m1\"]" });
                Extension.RvExtensionArgsImpl("UpdateMessages", new[] { "[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",0,\"m1\"]", "[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",2,\"m2\"]" });
                Extension.RvExtensionArgsImpl("UpdateMessageTemplates", new[] {
                "[\"maps2.plan-ops.fr#1\",1,\"EVASAN\",\"EVASAN\",\"https://maps2.plan-ops.fr/MessageTemplates/Details/1?t=6sRe0JNoF7pGTB_-uw_H5wsvDSgExzPY6ufXqVt_jjE\",[[\"EVASAN\",\"\",[]],[\"Line 1\",\"POSITION UTM\",[[\"\",\"Position UTM\",5]]],[\"Line 2\",\"INDICATIF / FREQUENCE\",[[\"\",\"Indicatif\",3],[\"\",\"Fréquence\",4]]],[\"Line 3\",\"NOMBRE ET PRIORITE DES BLESSES\",[[\"A\",\"P1 (urgent)\",1],[\"B\",\"P2 (urgence chir.)\",1],[\"C\",\"P2 (prioritaire)\",1]]],[\"Line 4\",\"EQUIPEMENTS NECESSAIRES\",[[\"A\",\"néant\",6],[\"B\",\"hélico\",6],[\"C\",\"désincar.\",6],[\"D\",\"O2\",6],[\"E\",\"Autres\",0]]],[\"Line 5\",\"NOMBRE ET TYPE DE BLESSES\",[[\"L\",\"couché\",1],[\"A\",\"ambulatoire\",1],[\"E\",\"accompagnant (obligatoire si enfant)\",1]]],[\"Line 6\",\"SECURITE ZONE\",[[\"N\",\"Sure\",6],[\"P\",\"ennemis possibles\",6],[\"E\",\"ennemis présent\",6],[\"X\",\"escorte armée\",6]]],[\"Line 7\",\"MARQUAGE ZPH\",[[\"A\",\"cyalum, PN2A\",6],[\"B\",\"fusée éclair.\",6],[\"C\",\"fumigène\",6],[\"D\",\"néant\",6],[\"E\",\"autre moyen\",0]]],[\"Line 8\",\"NATIONALITES DES BLESSES\",[[\"A\",\"Militaire FR/OTAN\",1],[\"D\",\"Civils\",1],[\"E\",\"Prisonnier\",1],[\"F\",\"Enfants\",1]]],[\"Line 9\",\"DESCRIPTIF DE LA ZONE ZPH\",[[null,\"si différent de Line 1\",0]]]]]",
                "[\"maps2.plan-ops.fr#2\",1,\"MEDEVAC\",\"MEDEVAC\",\"https://maps2.plan-ops.fr/MessageTemplates/Details/2?t=gpRXtrNNSpVSQk1HWpZqNsBJXSBnBFuvvXGb3W49SL4\",[[\"MEDEVAC\",\"\",[]],[\"Line 1\",\"LOCATION\",[[\"\",\"Grid of pickup zone\",5]]],[\"Line 2\",\"CALL SIGN & FREQ\",[[\"\",\"Call sign\",3],[\"\",\"Frequency\",4]]],[\"Line 3\",\"NUMBER OF PATIENTS/PRECEDENCE\",[[\"A\",\"URGENT Hospital under 90 min\",1],[\"B\",\"PRIORITY Hospital under 4 hours\",1],[\"C\",\"ROUTINE Hospital within 24 hours\",1]]],[\"Line 4\",\"SPECIAL EQUIPMENT REQUIRED\",[[\"A\",\"None\",6],[\"B\",\"Hoist (Winch)\",6],[\"C\",\"Extrication\",6],[\"D\",\"Ventilator\",6],[\"E\",\"Others\",0]]],[\"Line 5\",\"NUMBER TO BE CARRIED LYING/SITTING\",[[\"L\",\"Litter (Stretcher)\",1],[\"A\",\"Ambulatory (Walking)\",1],[\"E\",\"Escorts (e.g. for child patient)\",1]]],[\"Line 6\",\"SECURITY AT PICKUP ZONE (PZ)\",[[\"N\",\"No enemy\",6],[\"P\",\"Possible enemy\",6],[\"E\",\"Enemy in area\",6],[\"X\",\"Hot PZ - Armed escort required\",6]]],[\"Line 7\",\"PICKUP ZONE (PZ) MARKING METHOD\",[[\"A\",\"Panels\",6],[\"B\",\"Pyro\",6],[\"C\",\"Smoke\",6],[\"D\",\"None\",6],[\"E\",\"Other\",0]]],[\"Line 8\",\"NATIONALITY/STATUS\",[[\"A\",\"Military\",1],[\"D\",\"Civilian\",1],[\"E\",\"PW / Detainee\",1],[\"F\",\"Child\",1]]],[\"Line 9\",\"PICKUP ZONE (PZ) TERRAIN/OBSTACLES\",[[\"\",\"Terrain / obstacles\",0]]]]]" });
                
                loop = true;

                var update = FakeUpdateLoop();

                Console.ReadLine();

                loop = false;

                await update; 


            }
        }

        private static async Task FakeUpdateLoop()
        {
            while (loop)
            {
                Extension.RvExtensionArgsImpl("UpdateMarkers", new[] { "[\"v\",\"o2\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa\",\"\",\"Bison 01\",\"\",[6076.06,9287.43,178.459],359.76]", "[\"v\",\"o3\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa\",\"\",\"Bison 02\",\"\",[6087.6,9312.65,178.051],359.903]", "[\"v\",\"o5\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP03\",\"\",[6133.26,9267.66,170.838],0.0104469]", "[\"v\",\"o7\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa\",\"\\cTab\\img\\icon_air_contact_ca.paa\",\"Griffon 02\",\"\",[6120.25,9294.33,175.916],0.0432751]", "[\"v\",\"o8\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa\",\"\\cTab\\img\\icon_air_contact_ca.paa\",\"Griffon 01\",\"\",[6136.56,9317.35,176.434],0.00197615]", "[\"v\",\"o4\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP02\",\"\",[6196.91,9289.84,175.783],359.901]", "[\"v\",\"o6\",\"\\cTab\\img\\b_mech_inf_wheeled.paa\",\"\",\"SP04\",\"\",[6196.98,9255.88,168.648],359.997]", "[\"g\",\"o9\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP02\",\"\",[6179.84,9297.33,175.754],0,\"\"]", "[\"g\",\"o10\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP03\",\"\",[6133.7,9265.86,173.151],0,\"o5\"]", "[\"g\",\"o11\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP04\",\"\",[6177.97,9262.54,169.726],0,\"\"]", "[\"u\",\"m0\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:00\",\"\",[5198,9157],400]", "[\"u\",\"m1\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_mech_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:00\",\"\",[5273,9159],400]", "[\"u\",\"m2\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_motor_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:00\",\"\",[5352,9157],400]", "[\"u\",\"m3\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_armor.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:00\",\"\",[5426,9158],400]", "[\"u\",\"m4\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_air.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_1.paa\",\"12:00\",\"\",[5497,9162],400]", "[\"u\",\"m5\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_air.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:01\",\"\",[5578,9157],400]", "[\"u\",\"m6\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_plane.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"12:01\",\"\",[5648,9157],400]", "[\"u\",\"m7\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\o_unknown.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_1.paa\",\"12:01\",\"\",[5725,9154],180]", "[\"u\",\"m8\",\"\\cTab\\img\\o_inf_rifle.paa\",\"\",\"12:01\",\"\",[5194,9084],45]", "[\"u\",\"m9\",\"\\cTab\\img\\o_inf_mg.paa\",\"\",\"12:01\",\"\",[5271,9078],400]", "[\"u\",\"m10\",\"\\cTab\\img\\o_inf_at.paa\",\"\",\"12:02\",\"\",[5352,9081],400]", "[\"u\",\"m11\",\"\\cTab\\img\\o_inf_mmg.paa\",\"\",\"12:02\",\"\",[5434,9075],400]", "[\"u\",\"m12\",\"\\cTab\\img\\o_inf_mat.paa\",\"\",\"12:02\",\"\",[5503,9080],400]", "[\"u\",\"m13\",\"\\cTab\\img\\o_inf_aa.paa\",\"\",\"12:02\",\"\",[5582,9073],400]", "[\"u\",\"m14\",\"\\cTab\\img\\o_inf_mmortar.paa\",\"\",\"12:02\",\"\",[5654,9078],400]", "[\"u\",\"m15\",\"\\A3\\ui_f\\data\\map\\markers\\military\\join_CA.paa\",\"\",\"12:02\",\"\",[5189,9009],400]", "[\"u\",\"m16\",\"\\A3\\ui_f\\data\\map\\markers\\military\\circle_CA.paa\",\"\",\"12:02\",\"\",[5278,9004],400]", "[\"u\",\"m17\",\"\\A3\\ui_f\\data\\map\\mapcontrol\\Hospital_CA.paa\",\"\",\"12:02\",\"\",[5348,9005],400]", "[\"u\",\"m18\",\"\\A3\\ui_f\\data\\map\\markers\\military\\warning_CA.paa\",\"\",\"12:02\",\"\",[5426,9004],400]", "[\"u\",\"m19\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_hq.paa\",\"\",\"12:03\",\"\",[5178,8930],400]", "[\"u\",\"m20\",\"\\A3\\ui_f\\data\\map\\markers\\military\\end_CA.paa\",\"\",\"12:03\",\"\",[5266,8936],400]" });
                Extension.RvExtensionArgsImpl("UpdateMapMarkers", new[] {
                    "[[\"_USER_DEFINED #0/4/3\",[6184.42,9024.04,0],\"hd_dot\",\"ICON\",[1,1],0,\"Solid\",\"ColorBlack\",\"HELLO\",1],[\"_USER_DEFINED #0/5/3\",[6726.9,9003.73,0],\"hd_dot\",\"ICON\",[1,1],0,\"Solid\",\"ColorBlack\",\"WORLD\",1]]",
                    "[[\"_USER_DEFINED #0/3/3\",[5855.28,8989.5,5841.06,8993.57,5828.87,9005.76,5814.65,9028.11,5798.39,9066.71,5782.14,9115.47,5771.98,9204.87,5769.95,9304.42,5786.2,9416.17,5826.84,9511.66,5885.76,9599.03,5964.99,9682.33,6092.99,9751.41,6231.15,9775.79,6371.34,9767.66,6527.79,9720.93,6643.6,9647.79,6718.77,9527.92,6735.03,9403.98,6710.65,9286.14,6661.88,9233.31,6592.8,9204.87,6493.25,9198.77,6385.57,9221.12,6190.52,9318.65,6123.47,9385.7,6111.28,9434.46,6129.57,9475.09,6178.33,9499.47,6239.28,9491.35,6286.01,9458.84,6298.2,9420.23,6296.17,9379.6,6290.07,9365.38,6286.01,9363.35],\"Solid\",\"ColorBlack\",1]]"
                });
                for (int i = 1; i < 10; ++i)
                {
                    await Task.Delay(250);
                    Extension.RvExtensionArgsImpl("UpdatePosition", new[] { "6093", "9256.31", "171.876", "351.846", "[2035,6,24,12,0]", "\"o11\"" });
                }
                Extension.RvExtensionArgsImpl("UpdateMessages", new[] { "[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",1,\"m1\"]", "[\"12:00 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"Test\",2,\"m2\"]", "[\"12:01 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"EH BEH\",0,\"m3\"]", "[\"12:01 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"EH BEH\",2,\"m4\"]", "[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"test\",0,\"m5\"]", "[\"12:02 - SP01:1 ([1eGTD] Cpl. J.GrueArbre)\",\"test\",2,\"m6\"]" });
            }
        }

        static int Callback(string name, string function, string data)
        {
            Console.WriteLine(data);
            return 0;
        }
    }
}
