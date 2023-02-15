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
                            NextMenu = 11
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MedicalMenu,
                            NextMenu = 21
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_GeneralMenu,
                            NextMenu = 31
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_ControlPointMenu,
                            NextMenu = 101
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_ManoeuvreMenu,
                            NextMenu = 102
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SustainmentMenu,
                            NextMenu = 103
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
                            Select1 = 0, NextMenu = 121
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MechInfMenu,
                            Tooltip = SharedResource.STR_ctab_core_MechInfHint,
                            Select1 = 1, NextMenu = 12
                        },

                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MotoInfMenu,
                            Tooltip = SharedResource.STR_ctab_core_MotoInfHint,
                            Select1 = 2, NextMenu = 12
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_ArmorMenu,
                            Select1 = 3, NextMenu = 12
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_HelicopterMenu,
                            Select1 = 4, NextMenu = 12
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PlaneMenu,
                            Select1 = 5, NextMenu = 12
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_UnknownMenu,
                            Select1 = 6, NextMenu = 12
                        },

                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_Mine,
                            Select1 = 14
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_IED,
                            Select1 = 15
                        }
                    }
                },

                new Menu()
                {
                    MenuId = 12,
                    Items = new [] {
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SingularMenu,
                            NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_TeamMenu,
                            Tooltip = SharedResource.STR_ctab_core_TeamHint,
                            Select2 = 1, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SquadMenu,
                            Tooltip = SharedResource.STR_ctab_core_SquadHint,
                            Select2 = 2, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SectionMenu,
                            Tooltip = SharedResource.STR_ctab_core_SectionHint,
                            Select2 = 3, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PlatoonMenu,
                            Tooltip = SharedResource.STR_ctab_core_PlatoonHint,
                            Select2 = 4, NextMenu = 13
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
                            NextMenu = 14
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_TeamMenu,
                            Tooltip = SharedResource.STR_ctab_core_TeamHint,
                            Select2 = 1, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SquadMenu,
                            Tooltip = SharedResource.STR_ctab_core_SquadHint,
                            Select2 = 2, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SectionMenu,
                            Tooltip = SharedResource.STR_ctab_core_SectionHint,
                            Select2 = 3, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PlatoonMenu,
                            Tooltip = SharedResource.STR_ctab_core_PlatoonHint,
                            Select2 = 4, NextMenu = 13
                        }
                    }
                },

                new Menu()
                {
                    MenuId = 13,
                    Items = new [] {
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_StationaryMenu,
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_North,
                            Select3 = 1
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_NorthEast,
                            Select3 = 2
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_East,
                            Select3 = 3
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SouthEast,
                            Select3 = 4
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_South,
                            Select3 = 5
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_SouthWest,
                            Select3 = 6
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_West,
                            Select3 = 7
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_NorthWest,
                            Select3 = 8
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
                            Select1 = 7, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MGMenu,
                            Select1 = 8, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_ATMenu,
                            Select1 = 9, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_StaticMGMenu,
                            Select1 = 10, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_StaticATMenu,
                            Select1 = 11, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_StaticAAMenu,
                            Select1 = 13, NextMenu = 13
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MortarMenu,
                            Select1 = 12, NextMenu = 13
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
                            Select1 = 20
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_CCPMenu,
                            Tooltip = SharedResource.STR_ctab_core_CCPHint,
                            Select1 = 21
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_BASMenu,
                            Tooltip = SharedResource.STR_ctab_core_BASHint,
                            Select1 = 22
                        },
		                // Mass Casualty Incident
		                new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_MCIMenu,
                            Tooltip = SharedResource.STR_ctab_core_MCIHint,
                            Select1 = 23
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
                            Select1 = 30
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_LZMenu,
                            Tooltip = SharedResource.STR_ctab_core_LZHint,
                            Select1 = 31
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_DotMenu,
                            Select1 = 100,
                            NextMenu = 131
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_CircleMenu,
                            Select1 = 101,
                            NextMenu = 131
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
                            Select1 = 200
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointContact,
                            Select1 = 102
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointCoord,
                            Select1 = 103
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointCKP,
                            Select1 = 201,
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointSOS,
                            Select1 = 202,
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointEC,
                            Select1 = 203,
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointRLY,
                            Select1 = 204,
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointSP,
                            Select1 = 205,
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
                            Select1 = 104
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_CombatOutpost,
                            Select1 = 105
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointTarget,
                            Select1 = 106
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointPD,
                            Select1 = 206,
                        },
                    }

                },new Menu()
                {
                    MenuId = 103,
                    Items = new [] {

                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointCCP,
                            Select1 = 207
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointDET,
                            Select1 = 208
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointMED,
                            Select1 = 209
                        },
                        new MenuItem
                        {
                            Label = SharedResource.STR_ctab_core_PointR3P,
                            Select1 = 210,
                        },
                    }

                },
            };
        }
    }
}
