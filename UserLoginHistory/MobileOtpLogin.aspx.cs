using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace UserLoginHistory
{
    public partial class MobileOtpLogin : Page
    {
        private const int OtpLifetimeMinutes = 5;
        private const string OtpSessionKey = "MobileLoginOtp";
        private const string OtpExpirySessionKey = "MobileLoginOtpExpiry";
        private const string MobileSessionKey = "MobileLoginNumber";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSendOtp_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string mobileNumber = NormalizeMobileNumber(txtMobileNumber.Text);
            ClearOtp();
            if (!SendOtp(mobileNumber))
            {
                ShowMobileStep("Unable to send a code right now. Please try again later.");
                return;
            }

            ShowOtpStep(mobileNumber);
        }

        protected void btnResendOtp_Click(object sender, EventArgs e)
        {
            string mobileNumber = Convert.ToString(Session[MobileSessionKey]);
            if (String.IsNullOrEmpty(mobileNumber))
            {
                ShowMobileStep("Please enter your mobile number first.");
                return;
            }

            if (!SendOtp(mobileNumber))
            {
                ShowMessage("Unable to send a code right now. Please try again later.");
                return;
            }

            ShowOtpStep(mobileNumber);
        }

        protected void btnVerifyOtp_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string savedOtp = Convert.ToString(Session[OtpSessionKey]);
            DateTime expiry = Session[OtpExpirySessionKey] == null
                ? DateTime.MinValue
                : (DateTime)Session[OtpExpirySessionKey];

            if (String.IsNullOrEmpty(savedOtp) || DateTime.UtcNow > expiry)
            {
                ClearOtp();
                ShowMessage("This OTP has expired. Please request a new one.");
                return;
            }

            if (!String.Equals(savedOtp, txtOtp.Text.Trim(), StringComparison.Ordinal))
            {
                ShowMessage("The OTP is incorrect. Please try again.");
                return;
            }

            string mobileNumber = Convert.ToString(Session[MobileSessionKey]);
            ClearOtp();
            Session["MobileNumber"] = mobileNumber;
            Session["Authenticated"] = true;
            Response.Redirect("Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private bool SendOtp(string mobileNumber)
        {
            string otp = GenerateOtp();
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            string fromNumber = Environment.GetEnvironmentVariable("TWILIO_FROM_NUMBER");

            if (String.IsNullOrEmpty(accountSid) || String.IsNullOrEmpty(authToken) || String.IsNullOrEmpty(fromNumber))
            {
                Trace.TraceError("Twilio SMS settings are not configured.");
                return false;
            }

            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                string endpoint = "https://api.twilio.com/2010-04-01/Accounts/" +
                    Uri.EscapeDataString(accountSid) + "/Messages.json";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 15000;
                request.ReadWriteTimeout = 15000;
                request.Headers[HttpRequestHeader.Authorization] = "Basic " +
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(accountSid + ":" + authToken));

                string message = "Your verification code is " + otp + ". It expires in " +
                    OtpLifetimeMinutes + " minutes.";
                string formData = "To=" + HttpUtility.UrlEncode(mobileNumber) +
                    "&From=" + HttpUtility.UrlEncode(fromNumber) +
                    "&Body=" + HttpUtility.UrlEncode(message);
                byte[] requestBytes = Encoding.UTF8.GetBytes(formData);
                request.ContentLength = requestBytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                    {
                        return false;
                    }
                }

                Session[MobileSessionKey] = mobileNumber;
                Session[OtpSessionKey] = otp;
                Session[OtpExpirySessionKey] = DateTime.UtcNow.AddMinutes(OtpLifetimeMinutes);
                pnlMessage.Visible = true;
                lblMessage.Text = "A verification code has been sent.";
                return true;
            }
            catch (Exception exception)
            {
                Trace.TraceError("Twilio SMS request failed: " + exception.Message);
                return false;
            }

        }

        private void ShowOtpStep(string mobileNumber)
        {
            pnlMobile.Visible = false;
            pnlOtp.Visible = true;
            lblOtpDestination.Text = "Enter the 6-digit code sent to " + MaskMobileNumber(mobileNumber) + ".";
            txtOtp.Focus();
        }

        private void ShowMobileStep(string message)
        {
            pnlMobile.Visible = true;
            pnlOtp.Visible = false;
            ShowMessage(message);
        }

        private void ShowMessage(string message)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = Server.HtmlEncode(message);
        }

        private static string NormalizeMobileNumber(string mobileNumber)
        {
            return mobileNumber.Trim().Replace(" ", String.Empty).Replace("-", String.Empty);
        }

        private static string GenerateOtp()
        {
            byte[] bytes = new byte[4];
            using (RandomNumberGenerator generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(bytes);
            }

            uint value = BitConverter.ToUInt32(bytes, 0) % 1000000;
            return value.ToString("D6");
        }

        private static string MaskMobileNumber(string mobileNumber)
        {
            if (mobileNumber.Length <= 4)
            {
                return mobileNumber;
            }

            return new String('*', mobileNumber.Length - 4) + mobileNumber.Substring(mobileNumber.Length - 4);
        }

        private void ClearOtp()
        {
            Session.Remove(OtpSessionKey);
            Session.Remove(OtpExpirySessionKey);
        }
    }
}
