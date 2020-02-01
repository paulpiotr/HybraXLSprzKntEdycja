using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Hydra;
using System.Data.SqlClient;
using System.Collections;

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
        ClaWindow tab;
        ClaWindow tab2, tab3, button, tekst, usun, dropcombo, list;
        //MSSQL sql = new MSSQL();
        private string connetionString { get; set; }
        private SqlConnection sqlConnection { get; set; }
        private SqlCommand sqlCommand { get; set; }
        private SqlDataReader dataReader { get; set; }
        private Hashtable result { get; set; }
        private Exception exception { get; set; }
        private string message { get; set; }

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

            dropcombo = tab3.AllChildren.Add(ControlTypes.dropcombo);
            dropcombo.Visible = true;
            dropcombo.Bounds = new Rectangle(20, 40, 500, 15);

            button = tab3.AllChildren.Add(ControlTypes.button);
            button.Visible = true;
            button.Bounds = new Rectangle(530, 40, 60, 15);
            button.TextRaw = "Wyszukaj";

            list = tab3.AllChildren.Add(ControlTypes.list);
            list.Visible = true;
            list.Bounds = new Rectangle(20, 70, 570, 290);

            //tekst = tab3.AllChildren.Add(ControlTypes.text);
            //tekst.Bounds = new Rectangle(15, 60, 800, 50);
            //tekst.Visible = true;

            //usun = tab3.AllChildren.Add(ControlTypes.button);
            //usun.Visible = true;
            //usun.Bounds = new Rectangle(70, 40, 50, 15);
            //usun.TextRaw = "Usuń";

            return (true);

        }
        private bool OnOpenWindow(Procedures ProcID, int ControlID, Events Event)
        {
            System.Windows.Forms.MessageBox.Show("OnOpenWindow");
            //AddSubscription(false, dropcombo.Id, Events., new TakeEventDelegate(wypelnianie));
            AddSubscription(false, button.Id, Events.Accepted, new TakeEventDelegate(wypelnianie));
            //AddSubscription(false, usun.Id, Events.Accepted, new TakeEventDelegate(usuwanie));
            return (true);
        }

        private bool wypelnianie(Procedures ProcID, int ControlID, Events Event)
        {
            try
            {
                //while(ListaBranze().dataReader.Read())
                //{
                    //dropcombo.FromRaw = dropcombo.FromRaw + "|" + dataReader["SLW_Kategoria"];
                    //tekst.TextRaw = "\n" + dataReader["SLW_Kategoria"];
                //}
                ListaBranze();
                //foreach (Hashtable item in ListaBranze().result)
                //{
                //item["item"
                //}
                /*tekst.TextRaw = "dropcombo " +
                    dropcombo.Id.ToString() + " - " +
                    dropcombo.GetType().ToString() + " - " +
                    KntKarty.Knt_Akronim.ToString() + " - " +
                    KntKarty.Knt_Branza.ToString() + " - " +
                    dropcombo.ItemsRaw.GetType().ToString() + " - "
                ;*/
                //dropcombo.FromRaw = "string";
                //KntKarty.Knt_Akronim.ToString();
                //KntKarty.Knt_Branza.ToString();
                //Runtime.WindowController.UnlockThread();
                //System.Windows.Forms.Form form = new Form();
                //form.AddOwnedForm(new Form());
                //form.ShowDialog();
                //form.Show();
                //Runtime.Config.ExecSql(string Sql, bool Transakcja)
                Runtime.WindowController.LockThread();
                //dropcombo.ItemsRaw.Insert(0, "Nowa wartość");
                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message.ToString());
                return true;
            }
        }

        private XLSprzKntBranze ListaBranze()
        {
            try
            {
                string sql = "" +
                    "SELECT b.slw_id [SLW_ID],  b.SLW_WartoscS [Nazwa], bo.Opis [Opis] FROM CDN.el_CRMBranzeOpisy bo " +
                    "right join ( " +
                    "SELECT SLW_ID, SLW_WartoscS FROM CDN.slowniki " +
                    "WHERE SLW_Kategoria='Branże kontrahentów' and SLW_Aktywny = 1 and SLW_Predefiniowany = 0 " +

                    ") b on bo.branzaID = b.SLW_ID " +
                    "";
                string s = "";
                connetionString = Runtime.ActiveRuntime.Repository.Connection.ConnectionString.ToString();
                sqlConnection = new SqlConnection(connetionString);
                sqlConnection.Open();
                sqlCommand = new SqlCommand(sql, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                Hashtable item = new Hashtable();
                while (dataReader.Read())
                {
                    /*message = message + "\n" + dataReader["Nazwa"].ToString() + " " + dataReader["Opis"].ToString() + "\n";
                    item.Clear();
                    item.Add("Nazwa", dataReader["Nazwa"].ToString());
                    item.Add("Opis", dataReader["Opis"].ToString());
                    result.Add(dataReader["SLW_ID"], item);*/
                    s = s + dataReader["SLW_ID"].ToString() + "|" + dataReader["Nazwa"].ToString() + "|" + dataReader["Opis"].ToString() + "|.";
                }
                dataReader.Close();
                sqlConnection.Close();
                list.FormatRaw = "50L(1)~Id~|200L(2)~Nazwa~M|300L(2)~Opis~M";
                //list.FormatRaw = "10LJ@s1@[0L(2)|M*@s40@#3#]|M~Symbol~L[91L(2)*@s40@#8#]|M~Kod~L(2)[309L(2)|*@s255@#13#]|M~Nazwa~L(2)[57R(2)M*@s16@#18#18L(2)|*@s3@#23#](-2)|M~Cena netto~[32R(2)|*@s8@#28#](37)|M~J.m.~[60RM*@s12@#33#22L|*@s5@#38#]|M~Sprzedaż~[55RM*@s12@#43#22L|*@s5@#48#]|M~Magazyn~[44RM*@s12@#53#22L|*@s5@#58#]|M~Rezerwacje~0R(2)|M*~Księgowa~@n-15.2@#63#";
                list.ScrollRaw = "1";
                list.FromRaw = s;
                dropcombo.FromRaw = s;
                //tekst.TextRaw = s;
                //dropcombo.ScreenTextRaw = "01 Handel 01 Handel	firmy handlu hurtowego i detalicznego kupujące towar w  CT ELTECH do dalszej odsprzedaży";
                //dropcombo.DragIDRaw = "100";
                //dropcombo.OnAfterSaveInForm += Dropcombo_OnAfterSaveInForm;
                return this;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message.ToString());
                exception = e;
                return this;

            }
        }
        private bool usuwanie(Procedures ProcID, int ControlID, Events Event)
        {
            //tekst.TextRaw = " ";
            return true;
        }
    }

}
