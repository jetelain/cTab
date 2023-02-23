using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cTabWebApp.Models
{
    public class Menu
    {
        public MenuItem[] Items { get; set; }
        public int MenuId { get; set; }
        public string Class { get; private set; }
        public string Title { get; private set; }

        public static Menu[] CreateMenus()
        {
            return new[]
            {
                new Menu()
                {
                    Items = new[] {
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_EnemyMenu,
                            NextMenu = 11,
                            Preview = "/img/preview/10061000000000000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MedicalMenu,
                            NextMenu = 21,
                            Preview = "/img/mil_join.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_GeneralMenu,
                            NextMenu = 31,
                            Preview = "/img/mil_dot_blue.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_ControlPointMenu,
                            NextMenu = 101,
                            Preview = "/img/preview/10032500001301000000.png",
                            FeatureLevel= 1
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_ManoeuvreMenu,
                            NextMenu = 102,
                            Preview = "/img/preview/10032500001602050000.png",
                            FeatureLevel= 1
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SustainmentMenu,
                            NextMenu = 103,
                            Preview = "/img/preview/10032500003211000000.png",
                            FeatureLevel= 1
                        }
                    }
                },
                new Menu()
                {
                    MenuId = 11,
                    Items = new [] {
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_InfantryMenu,
                            Select1 = 0, NextMenu = 121,
                            Preview = "/img/preview/10061000001211000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MechInfMenu,
                            Tooltip = SharedResource.STR_ctab_core_MechInfHint,
                            Select1 = 1, NextMenu = 12,
                            Preview = "/img/preview/10061000001211020000.png"
                        },

                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MotoInfMenu,
                            Tooltip = SharedResource.STR_ctab_core_MotoInfHint,
                            Select1 = 2, NextMenu = 12,
                            Preview = "/img/preview/10061000001211040000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_ArmorMenu,
                            Select1 = 3, NextMenu = 12,
                            Preview = "/img/preview/10061000001205000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_HelicopterMenu,
                            Select1 = 4, NextMenu = 12,
                            Preview = "/img/preview/10061000001206000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PlaneMenu,
                            Select1 = 5, NextMenu = 12,
                            Preview = "/img/preview/10061000001208000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_UnknownMenu,
                            Select1 = 6, NextMenu = 12,
                            Preview = "/img/preview/10061000000000000000.png"
                        },

                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_Mine,
                            Select1 = 14,
                            Preview = "/img/preview/10061500002101000000.png",
                            FeatureLevel= 1
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_IED,
                            Select1 = 15,
                            Preview = "/img/preview/10061500002104000000.png",
                            FeatureLevel= 1
                        }
                    }
                },

                new Menu()
                {
                    MenuId = 12,
                    Items = new [] {
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_UnknownMenu,
                            NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_TeamMenu,
                            Tooltip = SharedResource.STR_ctab_core_TeamHint,
                            Select2 = 1, NextMenu = 13,
                            Preview = "/img/preview/size11.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SquadMenu,
                            Tooltip = SharedResource.STR_ctab_core_SquadHint,
                            Select2 = 2, NextMenu = 13,
                            Preview = "/img/preview/size12.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SectionMenu,
                            Tooltip = SharedResource.STR_ctab_core_SectionHint,
                            Select2 = 3, NextMenu = 13,
                            Preview = "/img/preview/size13.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PlatoonMenu,
                            Tooltip = SharedResource.STR_ctab_core_PlatoonHint,
                            Select2 = 4, NextMenu = 13,
                            Preview = "/img/preview/size14.png"
                        }
                    }
                },

                new Menu()
                {
                    MenuId = 121,
                    Items = new [] {
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SingularMenu,
                            NextMenu = 14,
                            Preview = "/img/preview/10061500001100000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_TeamMenu,
                            Tooltip = SharedResource.STR_ctab_core_TeamHint,
                            Select2 = 1, NextMenu = 13,
                            Preview = "/img/preview/size11.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SquadMenu,
                            Tooltip = SharedResource.STR_ctab_core_SquadHint,
                            Select2 = 2, NextMenu = 13,
                            Preview = "/img/preview/size12.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SectionMenu,
                            Tooltip = SharedResource.STR_ctab_core_SectionHint,
                            Select2 = 3, NextMenu = 13,
                            Preview = "/img/preview/size13.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PlatoonMenu,
                            Tooltip = SharedResource.STR_ctab_core_PlatoonHint,
                            Select2 = 4, NextMenu = 13,
                            Preview = "/img/preview/size14.png"
                        }
                    }
                },

                new Menu()
                {
                    MenuId = 13,
                    Class = "ctab-compass",
                    Title = SharedResource.WhereIsTheUnitMoving,
                    Items = new [] {
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_NorthWest,
                            Select3 = 8,
                            Preview = "/img/preview/NW.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_North,
                            Select3 = 1,
                            Preview = "/img/preview/N.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_NorthEast,
                            Select3 = 2,
                            Preview = "/img/preview/NE.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_West,
                            Select3 = 7,
                            Preview = "/img/preview/W.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_StationaryMenu,
                            Preview = "/img/preview/nope.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_East,
                            Select3 = 3,
                            Preview = "/img/preview/E.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SouthWest,
                            Select3 = 6,
                            Preview = "/img/preview/SW.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_South,
                            Select3 = 5,
                            Preview = "/img/preview/S.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SouthEast,
                            Select3 = 4,
                            Preview = "/img/preview/SE.png"
                        }
                    }
                },

                new Menu()
                {
                    MenuId = 14,
                    Items = new [] {
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_RifleMenu,
                            Select1 = 7, NextMenu = 13,
                            Preview = "/img/preview/10061500001100000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MGMenu,
                            Select1 = 8, NextMenu = 13,
                            Preview = "/img/preview/10062700001103010000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_ATMenu,
                            Select1 = 9, NextMenu = 13,
                            Preview = "/img/preview/10062700001103160000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_StaticMGMenu,
                            Select1 = 10, NextMenu = 13,
                            Preview = "/img/preview/10062700001103030000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_StaticATMenu,
                            Select1 = 11, NextMenu = 13,
                            Preview = "/img/preview/10062700001103070000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_StaticAAMenu,
                            Select1 = 13, NextMenu = 13,
                            Preview = "/img/preview/10061500001111000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MortarMenu,
                            Select1 = 12, NextMenu = 13,
                            Preview = "/img/preview/10062700001103140000.png"
                        }
                    }
                },

                new Menu()
                {
                    MenuId = 21,
                    Items = new [] {
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_CasualtyMenu,
                            Select1 = 20,
                            Preview = "/img/mil_join.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_CCPMenu,
                            Tooltip = SharedResource.STR_ctab_core_CCPHint,
                            Select1 = 21,
                            Preview = "/img/mil_circle.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_BASMenu,
                            Tooltip = SharedResource.STR_ctab_core_BASHint,
                            Select1 = 22,
                            Preview = "/img/preview/10032000001122020000.png"
                        },
		                // Mass Casualty Incident
		                new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MCIMenu,
                            Tooltip = SharedResource.STR_ctab_core_MCIHint,
                            Select1 = 23,
                            Preview = "/img/mil_warning.png"
                        }
                    }
                },

                new Menu()
                {
                    MenuId = 31,
                    Items = new [] {

                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_HQMenu,
                            Tooltip = SharedResource.STR_ctab_core_HQHint,
                            Select1 = 30,
                            Preview = "/img/preview/10031002000000000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_LZMenu,
                            Tooltip = SharedResource.STR_ctab_core_LZHint,
                            Select1 = 31,
                            Preview = "/img/mil_end.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_DotMenu,
                            Select1 = 100,
                            NextMenu = 131,
                            Preview = "/img/mil_dot_blue.png",
                            FeatureLevel= 1
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_CircleMenu,
                            Select1 = 101,
                            NextMenu = 131,
                            Preview = "/img/mil_circle_blue.png",
                            FeatureLevel= 1
                        }
                    }

                },

                new Menu()
                {
                    MenuId = 131,
                    Items = new [] {

                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_TSMenu,
                            Select2 = 0
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_AWithTSMenu,
                            Select2 = 1
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_BWithTSMenu,
                            Select2 = 2
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_CWithTSMenu,
                            Select2 = 3
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_DWithTSMenu,
                            Select2 = 4
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_EWithTSMenu,
                            Select2 = 5
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_FWithTSMenu,
                            Select2 = 6
                        }
                    }

                },


                new Menu()
                {
                    MenuId = 101,
                    Items = new [] {

                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointUnspec,
                            Select1 = 200,
                            Preview = "/img/preview/10032500001301000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointContact,
                            Select1 = 102,
                            Preview = "/img/preview/10032500001305000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointCoord,
                            Select1 = 103,
                            Preview = "/img/preview/10032500001306000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointCKP,
                            Select1 = 201,
                            Preview = "/img/preview/10032500001303000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointSOS,
                            Select1 = 202,
                            Preview = "/img/preview/10032500001308000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointEC,
                            Select1 = 203,
                            Preview = "/img/preview/10032500001309000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointRLY,
                            Select1 = 204,
                            Preview = "/img/preview/10032500001314000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointSP,
                            Select1 = 205,
                            Preview = "/img/preview/10032500001316000000.png"
                        }
                    }

                },



                new Menu()
                {
                    MenuId = 102,
                    Items = new [] {

                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_Outpost,
                            Select1 = 104,
                            Preview = "/img/preview/10032500001601000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_CombatOutpost,
                            Select1 = 105,
                            Preview = "/img/preview/10032500001602050000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointTarget,
                            Select1 = 106,
                            Preview = "/img/preview/10032500001603000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointPD,
                            Select1 = 206,
                            Preview = "/img/preview/10032500001604000000.png"
                        },
                    }

                },new Menu()
                {
                    MenuId = 103,
                    Items = new [] {

                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointCCP,
                            Select1 = 207,
                            Preview = "/img/preview/10032500003205000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointDET,
                            Select1 = 208,
                            Preview = "/img/preview/10032500003207000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointMED,
                            Select1 = 209,
                            Preview = "/img/preview/10032500003211000000.png"
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointR3P,
                            Select1 = 210,
                            Preview = "/img/preview/10032500003212000000.png"
                        },
                    }

                },
            };
        }
    }
}
