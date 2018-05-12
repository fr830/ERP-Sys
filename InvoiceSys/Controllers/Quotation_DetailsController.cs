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
    public class Quotation_DetailsController : Controller
    {
        private InvoiceSysDBContext db = new InvoiceSysDBContext();
        WebMatrix.Data.Database db2 = WebMatrix.Data.Database.Open("InvoiceSysDBContext");

        // GET: Quotation_Details
        public ActionResult Index(string status, string customerSearch, string creatorSearch, string quoNo)
        {
            String[] hiddenStatus = new String[] { "Draft", "Pending", "Reject", "Cancelled" };
            var quotation_Details = from m in db.Quotation_Details
                                    where m.approval_status != "Amended"
                                    select m;

            ViewBag.creator = DropDownList.CreateCreatorDropDownList();
            ViewBag.company = DropDownList.CreateCompanyList();
            ViewBag.quoNo = DropDownList.CreateQuotationNoList();

            if (String.IsNullOrEmpty(status))
            {
                quotation_Details = quotation_Details.Where(s => s.approval_status == "Pending");
            }

            if (LoginFunction.getUserTypeByUsername(Session["username"].ToString()) == "Customer")
            {
                quotation_Details = quotation_Details.Where(s => s.approval_status.Contains("Approve"));
                for (int i = 0; i < hiddenStatus.Length; i++)
                {
                    var hideStatus = hiddenStatus[i];
                    quotation_Details = quotation_Details.Where(s => s.status != hideStatus);
                }
            }

            if (!String.IsNullOrEmpty(status) && status != "All")
            {
                quotation_Details = quotation_Details.Where(s => s.status.Contains(status));
            }

            if (!string.IsNullOrEmpty(customerSearch))
            {
                quotation_Details = quotation_Details.Where(x => x.company_name == customerSearch);
            }


            if (!string.IsNullOrEmpty(creatorSearch))
            {
                quotation_Details = quotation_Details.Where(x => x.created_by == creatorSearch);
            }

            if (!string.IsNullOrEmpty(quoNo))
            {
                quotation_Details = quotation_Details.Where(x => x.quotation_no == quoNo);
            }


            if (!SAPermissionChecker.isSuperAdmin(Session["permission"].ToString()))
            {
                String companyName = LoginFunction.getCompanyByUsername(Session["username"].ToString());
                quotation_Details = quotation_Details.Where(x => x.company_name == companyName);
            }

            return View(quotation_Details);
        }


        // GET: Quotation_Details/Details/5
        public ActionResult Details(int? id, string needLayout)
        {
            if (!String.IsNullOrEmpty(needLayout))
            {
                ViewBag.layout = "No";
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Quotation_Details quotation_Details = db.Quotation_Details.Find(id);
            var result = db2.Query("SELECT * FROM quotation_elaboration WHERE quotation_no =@0 AND status !='Amended'", quotation_Details.quotation_no);
            ViewBag.result = result.Cast<dynamic>().ToList();
            if (quotation_Details == null)
            {
                return HttpNotFound();
            }
            return View(quotation_Details);
        }

        // GET: Quotation_Details/Create
        public ActionResult Create()
        {
            var running_no = db2.QueryValue("SELECT running_no FROM Running_no WHERE task_code = 'QUOID'");
            var quotation_no = "MTLQUO-" + DateTime.Now.Year.ToString() + "-" + running_no.ToString().PadLeft(3, '0');
            Quotation_Details quotation_details = new Quotation_Details();
            quotation_details.quotation_no = quotation_no;
            return View(quotation_details);
        }

        // POST: Quotation_Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "quotation_id,quotation_no,company_name,company_address,status,created_date,payment_term,attn_to")] Quotation_Details quotation_details,string[] item_description,string[] item_elaboration,string[] item_qty, string[] item_price,String Command,string[] item_no, string[] tax_code, string[] item_cost, string[] item_supplier)
        {
            if (ModelState.IsValid && (Command == "Submit"|| Command == "Save As Draft"))
            {
                if(Command != "Submit")
                {
                    quotation_details.status = "Draft";
                }
                else
                {
                    quotation_details.status = "Quotation Issued";
                }
                quotation_details.approval_status = "Pending";
                quotation_details.approval_date = DateTime.Now;
                quotation_details.modified_date = DateTime.Now;
                quotation_details.modified_by = quotation_details.modified_by == null ? Session["name"].ToString() : "";
                quotation_details.created_by = quotation_details.created_by == null ? Session["name"].ToString() : "";
                db.Quotation_Details.Add(quotation_details);

                try
                {
                    for (int i = 0; i < item_description.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(item_description[i].Trim()))
                        {
                            try
                            {
                                db2.Execute("INSERT INTO Quotation_Elaboration(quotation_no,item_description,item_elaboration,item_qty,item_price,status,created_date,item_no,tax_code,item_cost,item_supplier) VALUES (@0,@1,@2,@3,@4,'Current',CURRENT_TIMESTAMP,@5,@6,@7,@8)", quotation_details.quotation_no, item_description[i], item_elaboration[i], item_qty[i], item_price[i], item_no[i], tax_code[i], item_cost[i], item_supplier[i]);
                            }
                            catch
                            {
                                db2.Execute("INSERT INTO Quotation_Elaboration(quotation_no,item_description,item_elaboration,item_qty,item_price,status,created_date,item_no,tax_code) VALUES (@0,@1,@2,@3,@4,'Current',CURRENT_TIMESTAMP,@5,@6)", quotation_details.quotation_no, item_description[i], item_elaboration[i], item_qty[i], item_price[i], item_no[i], tax_code[i]);
                            }
                        }

                    }
                }
                catch
                {

                }


                db.SaveChanges();
                db2.Execute("UPDATE [InvoiceSys].[dbo].[Running_No] SET running_no = (running_no + 1) WHERE task_code = 'QUOID'");
                return RedirectToAction("Index");
            }
            return View(quotation_details);
        }

        // GET: Quotation_Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation_Details quotation_Details = db.Quotation_Details.Find(id);
            var result = db2.Query("SELECT * FROM quotation_elaboration WHERE quotation_no =@0 AND status !='Amended'",quotation_Details.quotation_no);
            ViewBag.result = result.Cast<dynamic>().ToList();
            if (quotation_Details == null)
            {
                return HttpNotFound();
            }
            return View(quotation_Details);
        }

        // POST: Quotation_Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "quotation_id,quotation_no,company_name,company_address,status,created_date,payment_term,remarks,created_by,attn_to")] Quotation_Details quotation_Details,string[] quotation_elab_id, string[] item_description, string[] item_elaboration, string[] item_qty, string[] item_price, string[] item_no, string[] tax_code, string[] item_cost, string[] item_supplier)
        {
            if (ModelState.IsValid)
            {
                //quotation_Details.created_by = quotation_Details.created_by == null ? Session["username"].ToString() : "";
                db.Entry(quotation_Details).State = EntityState.Modified;
                quotation_Details.approval_status = "Pending";
                quotation_Details.modified_date = DateTime.Now;
                quotation_Details.modified_by = quotation_Details.modified_by == null ? Session["name"].ToString() : "";
                quotation_Details.approval_date = DateTime.Now;
                db.SaveChanges();

                try
                {
                    if (quotation_elab_id != null)
                    {
                        for (int i = 0; i < quotation_elab_id.Length; i++)
                        {
                            db2.Execute("UPDATE Quotation_Elaboration SET status='Amended' WHERE quotation_id =@0", quotation_elab_id[i]);
                        }
                    }
                    try
                    {
                        for (int i = 0; i < item_description.Length; i++)
                        {
                            try
                            {
                                db2.Execute("INSERT INTO Quotation_Elaboration(quotation_no,item_description,item_elaboration,item_qty,item_price,status,created_date,item_no,tax_code,item_cost,item_supplier) VALUES (@0,@1,@2,@3,@4,'Current',CURRENT_TIMESTAMP,@5,@6,@7,@8)", quotation_Details.quotation_no, item_description[i], item_elaboration[i], item_qty[i], item_price[i], item_no[i], tax_code[i], item_cost[i], item_supplier[i]);
                            }
                            catch
                            {
                                db2.Execute("INSERT INTO Quotation_Elaboration(quotation_no,item_description,item_elaboration,item_qty,item_price,status,created_date,item_no,tax_code) VALUES (@0,@1,@2,@3,@4,'Current',CURRENT_TIMESTAMP,@5,@6)", quotation_Details.quotation_no, item_description[i], item_elaboration[i], item_qty[i], item_price[i], item_no[i], tax_code[i]);
                            }

                        }
                    }
                    catch
                    {

                    }

                }catch (Exception e)
                {
                    db2.Execute("INSERT INTO tbl_error_log(error_module,error_details) VALUES (@0,@1)", "Quotation Details Controller", e.ToString());
                }


        
                return RedirectToAction("Index");
            }
            return View(quotation_Details);
        }

        // GET: Quotation_Details/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation_Details quotation_Details = db.Quotation_Details.Find(id);
            if (quotation_Details == null)
            {
                return HttpNotFound();
            }
            return View(quotation_Details);
        }

        // POST: Quotation_Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quotation_Details quotation_Details = db.Quotation_Details.Find(id);
            db.Quotation_Details.Remove(quotation_Details);
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
            if(String.IsNullOrEmpty(companySearch)) // Make an intentional false statement so that no result will be queried
            {
                company_details = from m in db.Customer_PrivateInfo
                                  where 1 != 1
                                  select m;
            }
            return View(company_details);
        }

        public ActionResult downloadQuotation(int? id)
        {
            Quotation_Details quotation_Details = db.Quotation_Details.Find(id);
            return View(quotation_Details);
        }

        public ActionResult ApproveQuotation(int? id)
        {
            Quotation_Details qd = db.Quotation_Details.Find(id);
            qd.approval_status = "Approve";
            qd.status = "Approve";
            qd.approval_date = DateTime.Now;
            db.Entry(qd).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult RejectQuotation(int? id)
        {
            Quotation_Details qd = db.Quotation_Details.Find(id);
            qd.approval_status = "Reject";
            qd.status = "Draft";
            qd.approval_date = DateTime.Now;
            db.Entry(qd).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult TaskListSearch_PopUp(string ticketIDSearch, string client_served, string date)
        {
            ViewBag.ticketID = DropDownList.CreateTicketIDDropDownList();
            ViewBag.customerList = DropDownList.CreateCompanyList();
            ViewBag.user_type = LoginFunction.getUserTypeByUsername(Session["username"].ToString());
            var customer_worksheet = from m in db.Customer_Worksheet
                                     where m.approval_status == "Approve"
                                     select m;
            customer_worksheet = customer_worksheet.Where(s => s.isChargeable == true);
            if (!String.IsNullOrEmpty(ticketIDSearch))
            {
                customer_worksheet = customer_worksheet.Where(s => s.ticket_id.Contains(ticketIDSearch));
            }
            if (!String.IsNullOrEmpty(client_served))
            {
                customer_worksheet = customer_worksheet.Where(s => s.client_served.Contains(client_served));
            }
            if (!String.IsNullOrEmpty(date))
            {
                var date2 = Convert.ToDateTime(date);
                customer_worksheet = customer_worksheet.Where(s => s.working_date.Year == date2.Year);
                customer_worksheet = customer_worksheet.Where(s => s.working_date.Month == date2.Month);
                customer_worksheet = customer_worksheet.Where(s => s.working_date.Day == date2.Day);
            }

            if(String.IsNullOrEmpty(ticketIDSearch) && String.IsNullOrEmpty(client_served) && String.IsNullOrEmpty(date))
            {
                customer_worksheet = from m in db.Customer_Worksheet
                                     where 1 != 1
                                     select m;
            }

            return View(customer_worksheet.ToList());

        }

        public ActionResult statusUpdate(String statusSubmit, String remarks, int? id)
        {
            Quotation_Details qd = db.Quotation_Details.Find(id);
            qd.status = statusSubmit;
            qd.remarks = remarks;
            db.Entry(qd).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult multiplePublish(String dtl_id)
        {
            dynamic id_list = dtl_id.Split(',');

            for (int i = 0; i<id_list.Length; i++)
            {
                Quotation_Details qd = db.Quotation_Details.Find(Convert.ToInt32(id_list[i]));
                qd.approval_status = "Approve";
                qd.approval_date = DateTime.Now;
                qd.modified_by = Session["name"].ToString();
                db.Entry(qd).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult multipleStatusUpdate(String dtl_id,String status, String remarks)
        {
            dynamic id_list = dtl_id.Split(',');

            for (int i = 0; i < id_list.Length; i++)
            {
                Quotation_Details qd = db.Quotation_Details.Find(Convert.ToInt32(id_list[i]));
                qd.status = status;
                qd.remarks = remarks;
                qd.approval_status = "Approve";
                qd.approval_date = DateTime.Now;
                qd.modified_by = Session["name"].ToString();
                db.Entry(qd).State = EntityState.Modified;
                db.SaveChanges();
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
