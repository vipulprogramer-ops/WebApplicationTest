using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace UserLoginHistory
{
    public class AuditRepository
    {
        // =====================================================
        // Record failed login
        // =====================================================

        public FailedLoginResult RecordFailedLogin(
            int userId)
        {
            FailedLoginResult result = new FailedLoginResult();

            using (SqlConnection con = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("dbo.sp_RecordFailedLogin",con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(
                        "@p_UserId",
                        SqlDbType.Int).Value =
                        userId;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["FailedLoginAttempts"]
                                != DBNull.Value)
                            {
                                result.FailedAttempts =
                                    Convert.ToInt32(
                                        reader[
                                            "FailedLoginAttempts"]);
                            }

                            if (reader["LockoutUntil"]
                                != DBNull.Value)
                            {
                                result.LockoutUntil =
                                    Convert.ToDateTime(
                                        reader[
                                            "LockoutUntil"]);
                            }

                            if (reader["IsLocked"]
                                != DBNull.Value)
                            {
                                result.IsLocked =
                                    Convert.ToBoolean(
                                        reader["IsLocked"]);
                            }
                        }
                    }
                }
            }

            return result;
        }


        // =====================================================
        // Reset failed login
        // =====================================================

        public void ResetFailedLogin(int userId)
        {
            using (SqlConnection con = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("dbo.sp_ResetFailedLogin",con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(
                        "@p_UserId",
                        SqlDbType.Int).Value =
                        userId;

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }


        // =====================================================
        // Create login audit
        // =====================================================

        public void CreateLoginAudit(int? userId,string username,string ipAddress,byte isSuccess,string failureReason)
        {
            using (SqlConnection con = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("dbo.sp_CreateLoginAudit",con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@p_UserId",SqlDbType.Int).Value = userId.HasValue ? (object)userId.Value : DBNull.Value;


                    cmd.Parameters.Add(
                        "@p_Username",
                        SqlDbType.VarChar,
                        50).Value =
                        username;

                    cmd.Parameters.Add(
                        "@p_IPAddress",
                        SqlDbType.VarChar,
                        45).Value =
                        ipAddress;

                    cmd.Parameters.Add(
                        "@p_IsSuccess",
                        SqlDbType.TinyInt).Value =
                        isSuccess;

                    cmd.Parameters.Add(
                        "@p_FailureReason",
                        SqlDbType.VarChar,
                        255).Value =
                        failureReason == null
                            ? (object)DBNull.Value
                            : failureReason;

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }


    // =========================================================
    // Failed login result
    // =========================================================

    public class FailedLoginResult
    {
        public int FailedAttempts { get; set; }

        public bool IsLocked { get; set; }

        public DateTime? LockoutUntil { get; set; }
    }
}
