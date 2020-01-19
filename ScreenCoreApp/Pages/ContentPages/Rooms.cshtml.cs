using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infoscreen_Verwaltung.classes;
using ScreenCoreApp.Classes;
using System.Data;

namespace ScreenCoreApp
{
    public class RoomsModel : PageModel
    {
        public int Pagenum;
        public int item_start;
        public int item_end;
        public bool split;
        public DataTable Rooms;

        public IActionResult OnGet(string pagenum)
        {
            Pagenum = Convert.ToInt32(pagenum);

            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);
            Rooms = DatenbankAbrufen.RoomList(depID);

            item_start = (Pagenum-1) * 22;
            item_end = (Rooms.Rows.Count-1 > item_start + 21 ? item_start + 21 : Rooms.Rows.Count-1);

            split = (item_end - item_start > 10 ? true : false);

            return Page();
        }

        public string CreateSplitTable(int item_start, int item_end)
        {
            string html = "";

            html += @"
                <tr>
                    <th style='width:25%'>Raum</th>
                    <th style='width:50%'>Klasse</th>
                    <th style='width:25%'>Vorstand</th>
                </tr>";

            for(int i = item_start; i <=item_end; i++)
            {
                DataRow room = Rooms.Rows[i];
                string raum = StringHelper.ToValidRoomBuilding(room.ItemArray[0].ToString() + "-" + room.ItemArray[1].ToString(), 4, 2);
                string klasse = room.ItemArray[2].ToString();
                string kv = room.ItemArray[3].ToString();
                html += @"
                <tr>
                    <td>" + raum + @"</td>
                    <td>" + klasse + @"</td>
                    <td>" + kv + @"</td>
                </tr>";
            }

            for (int i = item_end+1; i <= item_start+10; i++)
            {
               
                html += @"
                    <tr style='opacity:0; background-color: transparent'>
                        <td>Layout_fill</td>
                        <td>Layout_fill</td>
                        <td>Layout_fill</td>
                    </tr>";
            }

            return html;
        }

    }
}