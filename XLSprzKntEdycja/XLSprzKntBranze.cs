﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Hydra;
using System.Data.SqlClient;
using System.Collections;
using System.ComponentModel;
using System.Data;

[assembly: CallbackAssemblyDescription(
    Name: "Branże na karcie kontrahentów",
    Description: "Zakładka Branże na karcie kontrahentów",
    Author: "ISK Sp Z o.o. [piotr.paul@isk.com.pl]",
    Version: "1.0",
    SystemVersion: "2019.3.0.0",
    Created: "28-01-2020")]

namespace XLSprzKntEdycja
{
    [SubscribeProcedure(Procedures.KntEdycja, "Zakładka Branże na karcie kontrahentów")]

    public class XLSprzKntBranze : Callback
    {
        ClaWindow parent;
        ClaWindow sheet;
        ClaWindow tab, tab2, tab3;
        ClaWindow button_search, button_load, button_save;
        ClaWindow dropcombo, list;
        ClaWindow text_SLW_ID, text_ElBranOpisID, text_opis, text_deb;
        /*private string connetionString { get; set; }
        private SqlConnection sqlConnection { get; set; }
        private SqlCommand sqlCommand { get; set; }
        private SqlDataReader dataReader { get; set; }*/
        public override void Init()
        {
            AddSubscription(true, 0, Events.JustAfterWindowOpening, new TakeEventDelegate(JustAfterWindowOpening));
            AddSubscription(false, 0, Events.OpenWindow, new TakeEventDelegate(OnOpenWindow));
        }

        public override void Cleanup() { }

        private bool JustAfterWindowOpening(Procedures ProcID, int ControlID, Events Event)
        {
            parent = GetWindow();
            sheet = parent.Children["?CurrentTab"];
            tab = sheet.Children.Add(ControlTypes.tab);
            tab.Visible = true;
            tab.TextRaw = "Branże kontrahentów";

            tab2 = tab.AllChildren.Add(ControlTypes.sheet);
            tab2.Visible = true;
            tab2.Bounds = new Rectangle(10, 20, 600, 350);

            tab3 = tab2.AllChildren.Add(ControlTypes.tab);
            tab3.Visible = true;
            tab3.TextRaw = "Wybór branży";
            tab3.Bounds = new Rectangle(10, 40, 600, 350);

            //text_deb = tab3.AllChildren.Add(ControlTypes.text);
            //text_deb.Visible = true;
            //text_deb.Bounds = new Rectangle(620, 40, 300, 200);

            text_SLW_ID = tab3.AllChildren.Add(ControlTypes.text);
            text_SLW_ID.Visible = true;
            text_SLW_ID.Bounds = new Rectangle(20, 40, 40, 15);

            text_ElBranOpisID = tab3.AllChildren.Add(ControlTypes.text);
            text_ElBranOpisID.Visible = true;
            text_ElBranOpisID.Bounds = new Rectangle(70, 40, 40, 15);

            text_opis = tab3.AllChildren.Add(ControlTypes.text);
            text_opis.Visible = true;
            text_opis.Bounds = new Rectangle(120, 40, 400, 30);

            button_save = tab3.AllChildren.Add(ControlTypes.button);
            button_save.Visible = true;
            button_save.Bounds = new Rectangle(530, 40, 60, 15);
            button_save.TextRaw = "Zapisz";

            dropcombo = tab3.AllChildren.Add(ControlTypes.dropcombo);
            dropcombo.Visible = true;
            dropcombo.Bounds = new Rectangle(20, 80, 430, 15);

            button_search = tab3.AllChildren.Add(ControlTypes.button);
            button_search.Visible = true;
            button_search.Bounds = new Rectangle(460, 80, 60, 15);
            button_search.TextRaw = "Wyszukaj";

            button_load = tab3.AllChildren.Add(ControlTypes.button);
            button_load.Visible = true;
            button_load.Bounds = new Rectangle(530, 80, 60, 15);
            button_load.TextRaw = "Wczytaj wszystkie";

            list = tab3.AllChildren.Add(ControlTypes.list);
            list.Visible = true;
            list.Bounds = new Rectangle(20, 100, 570, 250);

            ListFromRaw(null);
            list.OnAfterAccepted += List_OnAfterAccepted;

            return (true);
        }

        private bool List_OnAfterAccepted(Procedures ProcedureId, int ControlId, Events Event)
        {
            try
            {
                return ListFromRaw(dropcombo.ScreenTextRaw, Int32.Parse(list.SelectedRaw.ToString()));
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd: " + e.Message.ToString());
                throw new NotImplementedException();
            }
        }

        private bool OnOpenWindow(Procedures ProcID, int ControlID, Events Event)
        {
            AddSubscription(false, button_search.Id, Events.Accepted, new TakeEventDelegate(WyszukajBranze));
            AddSubscription(false, button_save.Id, Events.Accepted, new TakeEventDelegate(ZapiszBranze));
            AddSubscription(false, button_load.Id, Events.Accepted, new TakeEventDelegate(WczytajWszystkieBranze));
            AddSubscription(true, GetWindow().Children["?Cli_Zapisz"].Id, Events.Accepted, new TakeEventDelegate(ZapiszBranze));
            UstawBranze(ProcID, ControlID, Event);
            return (true);
        }

        private bool UstawBranze(Procedures ProcID, int ControlID, Events Event)
        {
            try
            {
                Int32 Knt_GIDNumer = Int32.Parse(KntKarty.Knt_GIDNumer.ToString());
                string sql = "" +
                    "SELECT TOP 1 * FROM [CDN].[el_CRMBranzeOpisy_KntKarty] eck " +
                    "INNER JOIN CDN.el_CRMBranzeOpisy ec ON " +
                    "ec.ElBranOpisID = eck.el_CRMBranzeOpisy_ElBranOpisID " +
                    "INNER JOIN CDN.Slowniki sl ON " +
                    "sl.SLW_ID = ec.branzaID " +
                    "WHERE eck.Knt_Karty_GIDNumer=" + Knt_GIDNumer;
                string connetionString = Runtime.ActiveRuntime.Repository.Connection.ConnectionString.ToString();
                SqlConnection sqlConnection = new SqlConnection(connetionString);
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Connection.Open();
                SqlDataReader dataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (dataReader.Read())
                {
                    text_SLW_ID.TextRaw = dataReader["SLW_ID"].ToString();
                    text_ElBranOpisID.TextRaw = dataReader["ElBranOpisID"].ToString();
                    text_opis.TextRaw = dataReader["SLW_WartoscS"].ToString() + " / " + dataReader["Opis"].ToString();
                }
                dataReader.Close();
                sqlCommand.Connection.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd: " + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }
        private bool SprawdzUstawionaBranza(Procedures ProcID, int ControlID, Events Event)
        {
            try
            {
                bool zapiszBranze = ZapiszBranze(ProcID, ControlID, Event);
                if (!zapiszBranze && KntKarty.Knt_Branza <= 0)
                {
                    //MessageBox.Show("Proszę wybrać branżę dla kontrahenta - zakładka Branże Kontrahentów", "Blokada zapisu kartoteki (brak w CRM)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                else
                {
                    Int32 Knt_GIDNumer = Int32.Parse(KntKarty.Knt_GIDNumer.ToString());
                    string sql = "SELECT TOP 1 Knt_Karty_GIDNumer FROM [CDN].[el_CRMBranzeOpisy_KntKarty] WHERE Knt_Karty_GIDNumer = " + Knt_GIDNumer;
                    string connetionString = Runtime.ActiveRuntime.Repository.Connection.ConnectionString.ToString();
                    SqlConnection sqlConnection = new SqlConnection(connetionString);
                    SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
                    if (!zapiszBranze && !dataReader.Read())
                    {
                        //MessageBox.Show("Proszę wybrać branżę dla kontrahenta - zakładka Branże Kontrahentów", "Blokada zapisu kartoteki (brak w SQL)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dataReader.Close();
                        sqlCommand.Connection.Close();
                        return false;
                    }
                    dataReader.Close();
                    sqlCommand.Connection.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd: " + e.Message);
                return false;
            }
        }

        private bool WczytajWszystkieBranze(Procedures ProcID, int ControlID, Events Event)
        {
            try
            {
                return ListFromRaw(null, 0);
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd: " + e.Message);
                return false;
            }
        }

        private bool ZapiszBranze(Procedures ProcID, int ControlID, Events Event)
        {
            try
            {
                KntKarty.Knt_Branza = Int32.Parse(text_SLW_ID.TextRaw.ToString());
                Int32 Knt_GIDNumer = Int32.Parse(KntKarty.Knt_GIDNumer.ToString());
                Int32 ElBranOpisID = Int32.Parse(text_ElBranOpisID.TextRaw.ToString());
                string sql = "" +
                    "IF EXISTS (SELECT Knt_Karty_GIDNumer FROM [CDN].[el_CRMBranzeOpisy_KntKarty] WHERE Knt_Karty_GIDNumer=" + Knt_GIDNumer + ")" +
                    "\n" +
                    "UPDATE [CDN].[el_CRMBranzeOpisy_KntKarty] SET " +
                        "Knt_Karty_GIDNumer=" + Knt_GIDNumer + ",el_CRMBranzeOpisy_ElBranOpisID=" + ElBranOpisID + " " +
                    "WHERE Knt_Karty_GIDNumer=" + Knt_GIDNumer +
                    "\n" +
                    "ELSE" +
                    "\n" +
                    "INSERT INTO" +
                    "\n" +
                    "[CDN].[el_CRMBranzeOpisy_KntKarty]" +
                    "\n" +
                    "(Knt_Karty_GIDNumer, el_CRMBranzeOpisy_ElBranOpisID)" +
                    "\n" +
                    "VALUES(" + Knt_GIDNumer + ", " + ElBranOpisID + ")";
                string connetionString = Runtime.ActiveRuntime.Repository.Connection.ConnectionString.ToString();
                SqlConnection sqlConnection = new SqlConnection(connetionString);
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
                return true;
            }
            catch (Exception e)
            {
                //MessageBox.Show("Proszę wybrać branżę dla kontrahenta z listy!", "Brak wyboru branży kontrahenta", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("Proszę wybrać branżę w zakładce Branże Kontrahentów! Należy wybrać branżę z listy i kliknć zapisz." + "\n" + e.Message.ToString(), "Blokada zapisu kartoteki (Branże Kontrahentów)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }
        private bool WyszukajBranze(Procedures ProcID, int ControlID, Events Event)
        {
            try
            {
                return ListFromRaw(dropcombo.ScreenTextRaw);
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd: " + e.Message.ToString());
                return false;
            }
        }

        private bool DropcomboFromRaw()
        {
            try
            {
                string sql = "" +
                        "SELECT b.slw_id [SLW_ID], b.SLW_WartoscS [Nazwa], bo.ElBranOpisID [ElBranOpisID], bo.Opis [Opis] FROM CDN.el_CRMBranzeOpisy bo " +
                            "RIGHT JOIN ( " +
                                "SELECT SLW_ID, SLW_WartoscS FROM CDN.slowniki " +
                                "WHERE SLW_Kategoria='Branże kontrahentów' and SLW_Aktywny = 1 and SLW_Predefiniowany = 0 " +
                            ") b on bo.branzaID = b.SLW_ID ";
                sql += " ORDER BY b.slw_id, b.SLW_WartoscS, bo.ElBranOpisID ";
                string dropcomboItems = "";
                Int32 k = 0;
                string connetionString = Runtime.ActiveRuntime.Repository.Connection.ConnectionString.ToString();
                SqlConnection sqlConnection = new SqlConnection(connetionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    if (k != Int32.Parse(dataReader["SLW_ID"].ToString()))
                    {
                        dropcomboItems = dropcomboItems + dataReader["Nazwa"].ToString() + "|";
                    }
                    k = Int32.Parse(dataReader["SLW_ID"].ToString());
                }
                dataReader.Close();
                sqlConnection.Close();
                dropcombo.FromRaw = dropcomboItems;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd: " + e.Message.ToString());
                return false;
            }
        }
        private bool ListFromRaw(string text = null, Int32 i = 0)
        {
            try
            {
                bool fillUpDropcombo = DropcomboFromRaw();
                string sql = "" +
                    "SELECT b.slw_id [SLW_ID], b.SLW_WartoscS [Nazwa], bo.ElBranOpisID [ElBranOpisID], bo.Opis [Opis] FROM CDN.el_CRMBranzeOpisy bo " +
                    "RIGHT JOIN ( " +
                        "SELECT SLW_ID, SLW_WartoscS FROM CDN.slowniki " +
                        "WHERE SLW_Kategoria='Branże kontrahentów' and SLW_Aktywny = 1 and SLW_Predefiniowany = 0 " +
                    ") b on bo.branzaID = b.SLW_ID ";
                if (null != text)
                {
                    sql += " WHERE b.SLW_WartoscS LIKE '%" + text.ToString() + "%' OR bo.Opis LIKE '%" + text.ToString() + "%' ";
                }
                sql += " ORDER BY b.slw_id, b.SLW_WartoscS, bo.ElBranOpisID ";
                string listaItems = "";
                Int32 j = 0;
                Int32 k = 0;
                string connetionString = Runtime.ActiveRuntime.Repository.Connection.ConnectionString.ToString();
                SqlConnection sqlConnection = new SqlConnection(connetionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    j++;
                    if (k == Int32.Parse(dataReader["SLW_ID"].ToString()))
                    {
                        listaItems = listaItems + " | |" + dataReader["Opis"].ToString() + "|";
                    }
                    else
                    {
                        listaItems = listaItems + dataReader["SLW_ID"].ToString() + "|" + dataReader["Nazwa"].ToString() + "|" + dataReader["Opis"].ToString() + "|";
                    }
                    k = Int32.Parse(dataReader["SLW_ID"].ToString());
                    if (i > 0 && j == i)
                    {
                        text_SLW_ID.TextRaw = dataReader["SLW_ID"].ToString();
                        text_ElBranOpisID.TextRaw = dataReader["ElBranOpisID"].ToString();
                        text_opis.TextRaw = dataReader["Nazwa"].ToString() + " / " + dataReader["Opis"].ToString();
                    }
                }
                dataReader.Close();
                sqlConnection.Close();
                list.FormatRaw = "20L(1)~Id~|150L(2)~Nazwa~M|300L(2)~Opis~M";
                list.ScrollRaw = "1";
                list.FromRaw = listaItems;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd: " + e.Message.ToString());
                return false;
            }
        }
    }
}