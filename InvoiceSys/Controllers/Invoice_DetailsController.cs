using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MrAng_Invoice.Models;

namespace MrAng_Invoice.Controllers
{
    public class Invoice_DetailsController : Controller
    {
        private InvoiceSysDBContext db = new InvoiceSysDBContext();
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");

        // GET: Invoice_Details
        public ActionResult Index(string status, string customerSearch, string creatorSearch, string invNo)
        {
            String[] hiddenStatus = new String[] {"Draft","Pending","Reject","Cancelled"};
            var invoice_Details = from m in db.Invoice_Details
                                  where m.approval_status != "Amended"
                                  select m;

            ViewBag.creator = DropDownList.CreateCreatorDropDownList();
            ViewBag.company = DropDownList.CreateCompanyList();
            ViewBag.invNo = DropDownList.CreateInvoiceNoList();

            if (String.IsNullOrEmpty(status))
            {
                invoice_Details = invoice_Details.Where(s => s.approval_status == "Pending");
            }
                if (LoginFunction.getUserTypeByUsername(Session["username"].ToString()) == "Customer")
            {
                invoice_Details = invoice_Details.Where(s => s.approval_status.Contains("Approve"));
                for(int i = 0; i< hiddenStatus.Length; i++)
                {
                    var hideStatus = hiddenStatus[i];
                    invoice_Details = invoice_Details.Where(s => s.status != hideStatus);
                }
            }
            if (!String.IsNullOrEmpty(status) && status != "All")
            {
                invoice_Details = invoice_Details.Where(s => s.status.Contains(status));
            }

            if (!string.IsNullOrEmpty(customerSearch))
            {
                invoice_Details = invoice_Details.Where(x => x.company_name == customerSearch);
            }

            if (!string.IsNullOrEmpty(creatorSearch))
            {
                invoice_Details = invoice_Details.Where(x => x.created_by == creatorSearch);
            }

            if (!string.IsNullOrEmpty(invNo))
            {
                invoice_Details = invoice_Details.Where(x => x.invoice_no == invNo);
            }

            if (!SAPermissionChecker.isSuperAdmin(Session["permission"].ToString()))
            {
                String companyName = LoginFunction.getCompanyByUsername(Session["username"].ToString());
                invoice_Details = invoice_Details.Where(x => x.company_name == companyName);
            }

            return View(invoice_Details);
        }


        // GET: Invoice_Details/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice_Details invoice_Details = db.Invoice_Details.Find(id);
            var result = db2.Query("SELECT * FROM invoice_elaboration WHERE invoice_no =@0 AND status !='Amended'", invoice_Details.invoice_no);
            ViewBag.result = result.Cast<dynamic>().ToList();
            if (invoice_Details == null)
            {
                return HttpNotFound();
            }
            return View(invoice_Details);
        }

        // GET: Invoice_Details/Create
        public ActionResult Create()
        {
            var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'INVID'");
            var invoice_no = "MTLINV-" + DateTime.Now.Year.ToString() + "-" + running_no.ToString().PadLeft(3, '0');
            Invoice_Details invoice_Details = new Invoice_Details();
            invoice_Details.invoice_no = invoice_no;
            return View(invoice_Details);
        }

        // POST: Invoice_Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "invoice_id,invoice_no,company_name,company_address,status,created_date,payment_term,attn_to")] Invoice_Details invoice_details, string[] item_description, string[] item_elaboration, string[] item_qty, string[] item_price, String Command, string[] item_no, string[] tax_code, string[] item_cost, string[] item_supplier)
        {

            if (ModelState.IsValid && (Command == "Submit" || Command == "Save As Draft"))
            {
                if (Command != "Submit")
                {
                    invoice_details.status = "Draft";
                }
                else
                {
                    invoice_details.status = "Invoice Issued";
                }
                invoice_details.approval_status = "Pending";
                invoice_details.approval_date = DateTime.Now;
                invoice_details.modified_date = DateTime.Now;
                invoice_details.modified_by = invoice_details.modified_by == null ? Session["name"].ToString() : "";
                invoice_details.created_by = invoice_details.created_by == null ? Session["name"].ToString() : "";
                db.Invoice_Details.Add(invoice_details);

                try
                {
                    for (int i = 0; i < item_description.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(item_description[i].Trim()))
                        {
                            try
                            {
                                db2.Execute("INSERT INTO Invoice_Elaboration(invoice_no,item_description,item_elaboration,item_qty,item_price,status,created_date,item_no,tax_code,item_cost, item_supplier) VALUES (@0,@1,@2,@3,@4,'Current',CURRENT_TIMESTAMP,@5,@6,@7,@8)", invoice_details.invoice_no, item_description[i], item_elaboration[i], item_qty[i], item_price[i], item_no[i], tax_code[i], item_cost[i], item_supplier[i]);
                            }
                            catch
                            {
                                db2.Execute("INSERT INTO Invoice_Elaboration(invoice_no,item_description,item_elaboration,item_qty,item_price,status,created_date,item_no,tax_code) VALUES (@0,@1,@2,@3,@4,'Current',CURRENT_TIMESTAMP,@5,@6)", invoice_details.invoice_no, item_description[i], item_elaboration[i], item_qty[i], item_price[i], item_no[i], tax_code[i]);
                            }
                        }

                    }
                }
                catch
                {

                }


                db.SaveChanges();
                db2.Execute("UPDATE [InvoiceSys].[dbo].[Running_No] SET running_no = (running_no + 1) WHERE task_code = 'INVID'");
                return RedirectToAction("Index");
            }

            return View(invoice_details);
        }

        // GET: invoice_Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice_Details invoice_Details = db.Invoice_Details.Find(id);
            var result = db2.Query("SELECT * FROM Invoice_Elaboration WHERE invoice_no =@0 AND status !='Amended'", invoice_Details.invoice_no);
            ViewBag.result = result.Cast<dynamic>().ToList();
            if (invoice_Details == null)
            {
                return HttpNotFound();
            }
            return View(invoice_Details);
        }

        // POST: Invoice_Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "invoice_id,invoice_no,company_name,company_address,status,created_date,payment_term,remarks,created_by,attn_to")] Invoice_Details invoice_details, string[] invoice_elab_id, string[] item_description, string[] item_elaboration, string[] item_qty, string[] item_price, string[] item_no , string[] tax_code, string[] item_cost, string[] item_supplier)
        {
            if (ModelState.IsValid)
            {
                //invoice_details.created_by = invoice_details.created_by == null ? Session["username"].ToString() : "";
                db.Entry(invoice_details).State = EntityState.Modified;
                invoice_details.approval_status = "Pending";
                invoice_details.modified_date = DateTime.Now;
                invoice_details.modified_by = invoice_details.modified_by == null ? Session["name"].ToString() : "";
                invoice_details.approval_date = DateTime.Now;
                db.SaveChanges();
                if (invoice_elab_id != null)
                {
                    for (int i = 0; i < invoice_elab_id.Length; i++)
                    {
                        db2.Execute("UPDATE Invoice_Elaboration SET status='Amended' WHERE invoice_id =@0", invoice_elab_id[i]);
                    }
                }
                try
                {
                    for (int i = 0; i < item_description.Length; i++)
                    {
                        try
                        {
                            db2.Execute("INSERT INTO Invoice_Elaboration(invoice_no,item_description,item_elaboration,item_qty,item_price,status,created_date,item_no,tax_code,item_cost,item_supplier) VALUES (@0,@1,@2,@3,@4,'Current',CURRENT_TIMESTAMP,@5,@6,@7,@8)", invoice_details.invoice_no, item_description[i], item_elaboration[i], item_qty[i], item_price[i], item_no[i], tax_code[i], item_cost[i], item_supplier[i]);
                        }
                        catch
                        {
                            db2.Execute("INSERT INTO Invoice_Elaboration(invoice_no,item_description,item_elaboration,item_qty,item_price,status,created_date,item_no,tax_code) VALUES (@0,@1,@2,@3,@4,'Current',CURRENT_TIMESTAMP,@5,@6)", invoice_details.invoice_no, item_description[i], item_elaboration[i], item_qty[i], item_price[i], item_no[i], tax_code[i]);
                        }

                    }
                }
                catch
                {

                }



                return RedirectToAction("Index");
            }
            return View(invoice_details);
        }

        // GET: Invoice_Details/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice_Details invoice_details = db.Invoice_Details.Find(id);
            if (invoice_details == null)
            {
                return HttpNotFound();
            }
            return View(invoice_details);
        }

        // POST: Invoice_Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice_Details invoice_details = db.Invoice_Details.Find(id);
            db.Invoice_Details.Remove(invoice_details);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CustomerSearch_PopUp(String companySearch)
        {
             ViewBag.company = DropDownList.CreateCompanyList();
            var company_details = from m in db.Customer_PrivateInfo
                                  where m.status != "Inactive"
                                  select m;
            if (!String.IsNullOrEmpty(companySearch))
            {
                company_details = company_details.Where(s => s.company_name.Contains(companySearch));
            }
            return View(company_details);
        }

        public ActionResult downloadInvoice(int? id)
        {
            Invoice_Details invoice_details = db.Invoice_Details.Find(id);
            return View(invoice_details);
        }

        public ActionResult ApproveInvoice(int? id)
        {
            Invoice_Details invDet = db.Invoice_Details.Find(id);
            invDet.approval_status = "Approve";
            invDet.status = "Approve";
            invDet.approval_date = DateTime.Now;
            db.Entry(invDet).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RejectInvoice(int? id)
        {
            Invoice_Details invDet = db.Invoice_Details.Find(id);
            invDet.approval_status = "Reject";
            invDet.status = "Draft";
            invDet.approval_date = DateTime.Now;
            db.Entry(invDet).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult TaskListSearch_PopUp(string ticketIDSearch)
        {
            ViewBag.ticketID=DropDownList.CreateTicketIDDropDownList();
            var customer_worksheet = from m in db.Customer_Worksheet
                                     where m.approval_status == "Approve"
                                     select m;
            if (!String.IsNullOrEmpty(ticketIDSearch))
            {
                customer_worksheet = customer_worksheet.Where(s => s.ticket_id.Contains(ticketIDSearch));
            }
            return View(customer_worksheet.ToList());

        }

        public ActionResult ImportQuotation_PopUp(string quotationNoSearch, string createdDate, string companyName)
        {
            ViewBag.quotaionNo=DropDownList.CreateQuotationNoDropDownList();
            ViewBag.companyName = DropDownList.CreateCompanyList();

            var quotation_list = from m in db.Quotation_Details
                                     where (m.approval_status == "Approve") 
                                     && (m.status == "PO Issued")
                                     select m;

            if (!String.IsNullOrEmpty(quotationNoSearch))
            {
                quotation_list = quotation_list.Where(s => s.quotation_no.Contains(quotationNoSearch));
            }
            if (!String.IsNullOrEmpty(companyName))
            {
                quotation_list = quotation_list.Where(s => s.company_name.Contains(companyName));
            }
            if (!String.IsNullOrEmpty(createdDate))
            {
                var created_date = Convert.ToDateTime(createdDate);
                quotation_list = quotation_list.Where(s => s.created_date.Day == created_date.Day);
                quotation_list = quotation_list.Where(s => s.created_date.Month == created_date.Month);
                quotation_list = quotation_list.Where(s => s.created_date.Year == created_date.Year);
            }
            //Intentional false statement to return empty list
            if(String.IsNullOrEmpty(quotationNoSearch) && String.IsNullOrEmpty(createdDate) && String.IsNullOrEmpty(companyName))
            {
                quotation_list = from m in db.Quotation_Details
                                 where 1 != 1
                                 select m;
            }

            return View(quotation_list.ToList());

        }

        public ActionResult ImportQuotation(string quotationNo, string created_date)
        {

            var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'INVID'");
            var invoice_no = "MTLINV-" + DateTime.Now.Year.ToString() + "-" + running_no.ToString().PadLeft(3, '0');

            var importQuotationQuery = db2.Execute("INSERT INTO Invoice_Details ([invoice_no],[company_name],[company_address],[remarks],[status],[approval_status],[approval_date],[created_by],[created_date],[modified_by],[modified_date],[payment_term],[attn_to]) SELECT @8,[company_name],[company_address],@7,[status],@1,@2,@3,@4,@5,@6,[payment_term],[attn_to] FROM Quotation_Details WHERE quotation_no = @0 AND approval_status = 'Approve'",quotationNo,"Pending",DateTime.Now, Session["name"], DateTime.Now, Session["name"], DateTime.Now,("Refer our quotation "+quotationNo+" dated "+created_date),invoice_no);

            var importQuotationElabQuery = db2.Execute("INSERT INTO Invoice_Elaboration ([invoice_no],[item_description],[item_elaboration],[item_qty],[item_price],[status],[created_date],[item_no],[tax_code],[item_cost],[item_supplier])SELECT @2,[item_description],[item_elaboration],[item_qty],[item_price],[status],@1,[item_no],[tax_code],[item_cost],[item_supplier] FROM Quotation_Elaboration WHERE quotation_no = @0 AND status = 'Current'", quotationNo, DateTime.Now, invoice_no);

            db2.Execute("UPDATE [InvoiceSys].[dbo].[Running_No] SET running_no = (running_no + 1) WHERE task_code = 'INVID'");

            return RedirectToAction("Index");
        }

        public ActionResult statusUpdate(String statusSubmit, String remarks, int? id)
        {
            Invoice_Details invDet = db.Invoice_Details.Find(id);
            invDet.status = statusSubmit;
            invDet.remarks = remarks;
            db.Entry(invDet).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult multiplePublish(String dtl_id)
        {
            dynamic id_list = dtl_id.Split(',');

            for (int i = 0; i < id_list.Length; i++)
            {
                Invoice_Details id = db.Invoice_Details.Find(Convert.ToInt32(id_list[i]));
                id.approval_status = "Approve";
                id.approval_date = DateTime.Now;
                id.modified_by = Session["name"].ToString();
                db.Entry(id).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult multipleStatusUpdate(String dtl_id, String status, String remarks)
        {
            dynamic id_list = dtl_id.Split(',');

            for (int i = 0; i < id_list.Length; i++)
            {
                Invoice_Details id = db.Invoice_Details.Find(Convert.ToInt32(id_list[i]));
                id.status = status;
                id.remarks = remarks;
                id.approval_status = "Approve";
                id.approval_date = DateTime.Now;
                id.modified_by = Session["name"].ToString();
                db.Entry(id).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult multipleQuotationImport(String quotation_no)
        {
            dynamic quotation_list = quotation_no.Split(',');

            for (int i = 0; i < quotation_list.Length; i++)
            {
                try
                {
                    var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'INVID'");
                    var invoice_no = "MTLINV-" + DateTime.Now.Year.ToString() + "-" + running_no.ToString().PadLeft(3, '0');
                    var quotation_created_date = db2.QueryValue("SELECT created_date FROM Quotation_Details WHERE quotation_no = @0", quotation_list[i]);

                    var importQuotationQuery = db2.Execute("INSERT INTO Invoice_Details ([invoice_no],[company_name],[company_address],[remarks],[status],[approval_status],[approval_date],[created_by],[created_date],[modified_by],[modified_date],[payment_term],[attn_to]) SELECT @8,[company_name],[company_address],@7,[status],@1,@2,@3,@4,@5,@6,[payment_term],[attn_to] FROM Quotation_Details WHERE quotation_no = @0 AND approval_status = 'Approve'", quotation_list[i], "Pending", DateTime.Now, Session["name"], DateTime.Now, Session["name"], DateTime.Now, ("Refer our quotation " + quotation_list[i] + " dated " + quotation_created_date.ToString("dd-MMMM-yyyy")), invoice_no);

                    var importQuotationElabQuery = db2.Execute("INSERT INTO Invoice_Elaboration ([invoice_no],[item_description],[item_elaboration],[item_qty],[item_price],[status],[created_date],[item_no],[tax_code],[item_cost],[item_supplier])SELECT @2,[item_description],[item_elaboration],[item_qty],[item_price],[status],@1,[item_no],[tax_code],[item_cost],[item_supplier] FROM Quotation_Elaboration WHERE quotation_no = @0 AND status = 'Current'", quotation_list[i], DateTime.Now, invoice_no);

                    db2.Execute("UPDATE [InvoiceSys].[dbo].[Running_No] SET running_no = (running_no + 1) WHERE task_code = 'INVID'");
                }catch(Exception e)
                {
                    System.IO.File.AppendAllText("D:/Test.log", e.ToString());
                }

            }
                return RedirectToAction("Index");
            
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
