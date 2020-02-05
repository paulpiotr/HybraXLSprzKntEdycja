using System;
using Hydra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
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
        ClaWindow button_search, button_load, button_save, button_add, button_remove;
        ClaWindow dropcombo, list, item;
        ClaWindow text_SLW_ID, text_ElBranOpisID, text_opis;

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

            text_SLW_ID = tab3.AllChildren.Add(ControlTypes.text);
            text_SLW_ID.Visible = false;
            text_SLW_ID.Bounds = new Rectangle(20, 40, 40, 15);

            text_ElBranOpisID = tab3.AllChildren.Add(ControlTypes.text);
            text_ElBranOpisID.Visible = false;
            text_ElBranOpisID.Bounds = new Rectangle(70, 40, 40, 15);

            item = tab3.AllChildren.Add(ControlTypes.stringcontrol);
            item.Visible = true;
            item.Bounds = new Rectangle(20, 40, 45, 15);
            item.TextRaw = " Branża: ";

            text_opis = tab3.AllChildren.Add(ControlTypes.text);
            text_opis.Visible = true;
            text_opis.Bounds = new Rectangle(70, 40, 530, 15);
            text_opis.ReadOnlyRaw = "1";

            /*button_save = tab3.AllChildren.Add(ControlTypes.button);
            button_save.Visible = true;
            button_save.Bounds = new Rectangle(520, 40, 80, 15);
            button_save.TextRaw = "Zapisz branże";

            button_add = tab3.AllChildren.Add(ControlTypes.button);
            button_add.Visible = true;
            button_add.Bounds = new Rectangle(520, 60, 40, 15);
            button_add.TextRaw = "Dodaj";

            button_remove = tab3.AllChildren.Add(ControlTypes.button);
            button_remove.Visible = true;
            button_remove.Bounds = new Rectangle(560, 60, 40, 15);
            button_remove.TextRaw = "Usuń";*/

            item = tab3.AllChildren.Add(ControlTypes.stringcontrol);
            item.Visible = true;
            item.Bounds = new Rectangle(20, 60, 45, 15);
            item.TextRaw = " Filtr: ";

            dropcombo = tab3.AllChildren.Add(ControlTypes.dropcombo);
            dropcombo.Visible = true;
            dropcombo.Bounds = new Rectangle(70, 60, 370, 15);

            button_search = tab3.AllChildren.Add(ControlTypes.button);
            button_search.Visible = true;
            button_search.Bounds = new Rectangle(450, 60, 60, 15);
            button_search.TextRaw = "Wyszukaj";

            button_load = tab3.AllChildren.Add(ControlTypes.button);
            button_load.Visible = true;
            button_load.Bounds = new Rectangle(520, 60, 80, 15);
            button_load.TextRaw = "Wyczyść filtr";

            list = tab3.AllChildren.Add(ControlTypes.list);
            list.Visible = true;
            list.Bounds = new Rectangle(20, 80, 580, 280);

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
            AddSubscription(true, button_search.Id, Events.Accepted, new TakeEventDelegate(WyszukajBranze));
            /*AddSubscription(true, button_save.Id, Events.Accepted, new TakeEventDelegate(ZapiszBranze));*/
            AddSubscription(true, button_load.Id, Events.Accepted, new TakeEventDelegate(WczytajWszystkieBranze));
            /*AddSubscription(true, button_add.Id, Events.Accepted, new TakeEventDelegate(DodajOpis));
            AddSubscription(true, button_remove.Id, Events.Accepted, new TakeEventDelegate(UsunOpis));*/
            AddSubscription(true, GetWindow().Children["?Cli_Zapisz"].Id, Events.Accepted, new TakeEventDelegate(ZapiszBranze));
            UstawBranze(ProcID, ControlID, Event);
            return (true);
        }

        private bool UsunOpis(Procedures ProcID, int ControlID, Events Event)
        {
            try
            {
                var result = MessageBox.Show(
                    "Usunć " + text_opis.TextRaw.ToString() + " id: " + text_ElBranOpisID.TextRaw.ToString(),
                    "Usuwanie Opisu",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string sql = "" +
                        "DELETE FROM" +
                        "\n" +
                        "[CDN].[el_CRMBranzeOpisy]" +
                        "\n" +
                        "WHERE ElBranOpisID=" + Int32.Parse(text_ElBranOpisID.TextRaw.ToString());
                    SqlConnection sqlConnection = Runtime.ActiveRuntime.Repository.Connection.CreateCommand().Connection;
                    SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
                return WyszukajBranze(ProcID, ControlID, Event);
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd: " + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        private bool DodajOpis(Procedures ProcID, int ControlID, Events Event)
        {
            try
            {
                string sql = "" +
                    "INSERT INTO" +
                    "\n" +
                    "[CDN].[el_CRMBranzeOpisy]" +
                    "\n" +
                    "(branzaID, Opis)" +
                    "\n" +
                    "VALUES(" + Int32.Parse(text_SLW_ID.TextRaw.ToString()) + ", '" + text_opis.TextRaw.ToString() + "')";
                SqlConnection sqlConnection = Runtime.ActiveRuntime.Repository.Connection.CreateCommand().Connection;
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
                return WyszukajBranze(ProcID, ControlID, Event);
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd: " + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        private bool UstawBranze(Procedures ProcID, int ControlID, Events Event)
        {
            try
            {
                Int32 Knt_GIDNumer = Int32.Parse(KntKarty.Knt_GIDNumer.ToString());
                string sql = "" +
                    "SELECT TOP 1 * FROM [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] eck " +
                    "INNER JOIN CDN.el_CRMBranzeOpisy ec ON " +
                    "ec.ElBranOpisID = eck.el_CRMBranzeOpisy_ElBranOpisID " +
                    "INNER JOIN CDN.Slowniki sl ON " +
                    "sl.SLW_ID = ec.branzaID " +
                    "WHERE eck.Knt_Karty_GIDNumer=" + Knt_GIDNumer;
                SqlConnection sqlConnection = Runtime.ActiveRuntime.Repository.Connection.CreateCommand().Connection;
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                SqlDataReader dataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (dataReader.Read())
                {
                    text_SLW_ID.TextRaw = dataReader["SLW_ID"].ToString();
                    text_ElBranOpisID.TextRaw = dataReader["ElBranOpisID"].ToString();
                    text_opis.TextRaw = dataReader["SLW_WartoscS"].ToString()/* + " / " + dataReader["Opis"].ToString()*/;
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
                    return false;
                }
                else
                {
                    Int32 Knt_GIDNumer = Int32.Parse(KntKarty.Knt_GIDNumer.ToString());
                    string sql = "SELECT TOP 1 Knt_Karty_GIDNumer FROM [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] WHERE Knt_Karty_GIDNumer = " + Knt_GIDNumer;
                    SqlConnection sqlConnection = Runtime.ActiveRuntime.Repository.Connection.CreateCommand().Connection;
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                    SqlDataReader dataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
                    if (!zapiszBranze && !dataReader.Read())
                    {
                        dataReader.Close();
                        sqlConnection.Close();
                        return false;
                    }
                    dataReader.Close();
                    sqlConnection.Close();
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
                dropcombo.ScreenTextRaw = " ";
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
                    "IF EXISTS (SELECT Knt_Karty_GIDNumer FROM [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] WHERE Knt_Karty_GIDNumer=" + Knt_GIDNumer + ")" +
                    "\n" +
                    "UPDATE [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] SET " +
                        "Knt_Karty_GIDNumer=" + Knt_GIDNumer + ",el_CRMBranzeOpisy_ElBranOpisID=" + ElBranOpisID + " " +
                    "WHERE Knt_Karty_GIDNumer=" + Knt_GIDNumer +
                    "\n" +
                    "ELSE" +
                    "\n" +
                    "INSERT INTO" +
                    "\n" +
                    "[CDN].[ISK_el_CRMBranzeOpisy_KntKarty]" +
                    "\n" +
                    "(Knt_Karty_GIDNumer, el_CRMBranzeOpisy_ElBranOpisID)" +
                    "\n" +
                    "VALUES(" + Knt_GIDNumer + ", " + ElBranOpisID + ")";
                SqlConnection sqlConnection = Runtime.ActiveRuntime.Repository.Connection.CreateCommand().Connection;
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Proszę wybrać branżę w zakładce Branże Kontrahentów! Należy wybrać branżę z listy i kliknć zapisz.", "Blokada zapisu kartoteki (Branże Kontrahentów)", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                        "SELECT s.slw_id [SLW_ID], s.SLW_WartoscS [Nazwa] FROM [CDN].[Slowniki] s " +
                        "WHERE s.SLW_Kategoria='Branże kontrahentów' AND s.SLW_Aktywny = 1 AND s.SLW_Predefiniowany = 0 " +
                        "ORDER BY s.SLW_ID, s.SLW_WartoscS";
                string dropcomboItems = "";
                Int32 k = 0;
                SqlConnection sqlConnection = Runtime.ActiveRuntime.Repository.Connection.CreateCommand().Connection;
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

        private bool MessageBoxAskTextRaw(string name)
        {
            try
            {
                var result = MessageBox.Show(
                    "Czy ustawić branże: [" + name + "]",
                    "Ustawianie branży",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                ;
                if (result == DialogResult.Yes)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        private bool ListFromRaw(string text = null, Int32 i = 0)
        {
            try
            {
                bool dropcomboFromRaw = DropcomboFromRaw();
                string sql = "" +
                        "SELECT s.SLW_ID [SLW_ID], s.SLW_WartoscS [Nazwa], o.ElBranOpisID [ElBranOpisID], COALESCE(o.Opis, 'Brak danych')[Opis] " +
                        "FROM [CDN].[el_CRMBranzeOpisy] o " +
                        "RIGHT JOIN( " +
                            "SELECT * FROM[CDN].[Slowniki] s " +
                            "WHERE s.SLW_Kategoria = 'Branże kontrahentów' AND s.SLW_Aktywny = 1 AND s.SLW_Predefiniowany = 0" +
                        ") s ON s.SLW_ID = o.branzaID" +
                        "";
                if (null != text)
                {
                    sql += " WHERE s.SLW_WartoscS LIKE '%" + text.ToString() + "%' OR o.Opis LIKE '%" + text.ToString() + "%' ";
                }
                sql += " ORDER BY s.SLW_ID, s.SLW_WartoscS, o.ElBranOpisID ";
                string listaItems = "";
                Int32 j = 0;
                Int32 k = 0;
                SqlConnection sqlConnection = Runtime.ActiveRuntime.Repository.Connection.CreateCommand().Connection;
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    j++;
                    if (k == Int32.Parse(dataReader["SLW_ID"].ToString()))
                    {
                        listaItems = listaItems + " |" + dataReader["Opis"].ToString() + "|";
                    }
                    else
                    {
                        listaItems = listaItems + /*dataReader["SLW_ID"].ToString() + "|" + */dataReader["Nazwa"].ToString() + "|" + dataReader["Opis"].ToString() + "|";
                    }
                    k = Int32.Parse(dataReader["SLW_ID"].ToString());
                    if (i > 0 && j == i && MessageBoxAskTextRaw(dataReader["Nazwa"].ToString()))
                    {
                        text_SLW_ID.TextRaw = dataReader["SLW_ID"].ToString();
                        text_ElBranOpisID.TextRaw = dataReader["ElBranOpisID"].ToString();
                        text_opis.TextRaw = dataReader["Nazwa"].ToString()/* + " / " + dataReader["Opis"].ToString()*/;
                    }
                }
                dataReader.Close();
                sqlConnection.Close();
                /*list.FormatRaw = "20L(1)~Id~|150L(2)~Nazwa~M|300L(2)~Opis~M";*/
                list.FormatRaw = "150L(2)~Nazwa~M|300L(2)~Opis~M";
                list.ScrollRaw = "1";
                list.VScrollRaw = "1";
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