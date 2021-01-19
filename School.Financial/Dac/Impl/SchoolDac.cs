using MySql.Data.MySqlClient;
using School.Financial.Models;
using System;
using System.Collections.Generic;

namespace School.Financial.Dac.Impl
{
    public class SchoolDac : ISchoolDac
    {
        private readonly SchoolFinancialContext context;

        public SchoolDac(SchoolFinancialContext context)
        {
            this.context = context;
        }

        public IEnumerable<SchoolData> Get()
        {
            var list = new List<SchoolData>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from School", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(ReadData(reader));
                    }
                }
            }
            return list;
        }

        public SchoolData Get(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from School where sc_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return ReadData(reader);
                    }
                }
            }
            return null;
        }

        public int Insert(SchoolData data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `School` (" +
                    "`sc_id`, `smis`, `sc_code`, `sc_name`, `areacode`, `sc_network`, `aumphur`, `province`, `spt`, `add1`, " +
                    "`add2`, `tumbol`, `p_code`, `tel`, `low_class`, `top_clsass`, `lat`, `lng`, `VatId`) " +
                    "VALUES (" +
                    "@sc_id,@smis,@sc_code,@sc_name,@areacode,@sc_network,@aumphur,@province,@spt,@add1," +
                    "@add2,@tumbol,@p_code,@tel,@low_class,@top_clsass,@lat,@lng,@VatId)", conn);
                cmd.Parameters.AddWithValue("@sc_id", data.sc_id);
                cmd.Parameters.AddWithValue("@smis", data.smis);
                cmd.Parameters.AddWithValue("@sc_code", data.sc_code);
                cmd.Parameters.AddWithValue("@sc_name", data.sc_name);
                cmd.Parameters.AddWithValue("@areacode", data.areacode);
                cmd.Parameters.AddWithValue("@sc_network", data.sc_network);
                cmd.Parameters.AddWithValue("@aumphur", data.aumphur);
                cmd.Parameters.AddWithValue("@province", data.province);
                cmd.Parameters.AddWithValue("@spt", data.spt);
                cmd.Parameters.AddWithValue("@add1", data.add1);
                cmd.Parameters.AddWithValue("@add2", data.add2);
                cmd.Parameters.AddWithValue("@tumbol", data.tumbol);
                cmd.Parameters.AddWithValue("@p_code", data.p_code);
                cmd.Parameters.AddWithValue("@tel", data.tel);
                cmd.Parameters.AddWithValue("@low_class", data.low_class);
                cmd.Parameters.AddWithValue("@top_clsass", data.top_class);
                cmd.Parameters.AddWithValue("@lat", data.lat);
                cmd.Parameters.AddWithValue("@lng", data.lng);
                cmd.Parameters.AddWithValue("@VatId", data.VatId);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Update(SchoolData data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `School` SET (" +
                    "`smis`=@smis,`sc_code`=@sc_code,`sc_name`=@sc_name,`areacode`=@areacode,`sc_network`=@sc_network,`aumphur`=@aumphur,`province`=@province,`spt`=@spt,`add1`=@add1" +
                    ",`add2`=@add2,`tumbol`=@tumbol,`p_code`=@p_code,`tel`=@tel,`low_class`=@low_class,`top_clsass`=@top_clsass,`lat`=@lat,`lng`=@lng,`VatId`=@VatId)" +
                    "WHERE `sc_id`=@Id", conn);
                cmd.Parameters.AddWithValue("@sc_id", data.sc_id);
                cmd.Parameters.AddWithValue("@smis", data.smis);
                cmd.Parameters.AddWithValue("@sc_code", data.sc_code);
                cmd.Parameters.AddWithValue("@sc_name", data.sc_name);
                cmd.Parameters.AddWithValue("@areacode", data.areacode);
                cmd.Parameters.AddWithValue("@sc_network", data.sc_network);
                cmd.Parameters.AddWithValue("@aumphur", data.aumphur);
                cmd.Parameters.AddWithValue("@province", data.province);
                cmd.Parameters.AddWithValue("@spt", data.spt);
                cmd.Parameters.AddWithValue("@add1", data.add1);
                cmd.Parameters.AddWithValue("@add2", data.add2);
                cmd.Parameters.AddWithValue("@tumbol", data.tumbol);
                cmd.Parameters.AddWithValue("@p_code", data.p_code);
                cmd.Parameters.AddWithValue("@tel", data.tel);
                cmd.Parameters.AddWithValue("@low_class", data.low_class);
                cmd.Parameters.AddWithValue("@top_clsass", data.top_class);
                cmd.Parameters.AddWithValue("@lat", data.lat);
                cmd.Parameters.AddWithValue("@lng", data.lng);
                cmd.Parameters.AddWithValue("@VatId", data.VatId);

                cmd.ExecuteNonQuery();
            }
        }

        public int Upsert(SchoolData data)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO `school`(" +
                    "`sc_id`, `smis`, `sc_code`, `sc_name`, `areacode`, `sc_network`, `aumphur`, `province`, `spt`, `add1`, " +
                    "`add2`, `tumbol`, `p_code`, `tel`, `low_class`, `top_clsass`, `lat`, `lng`, `VatId`) " +
                    "VALUES (" +
                    "@sc_id,@smis,@sc_code,@sc_name,@areacode,@sc_network,@aumphur,@province,@spt,@add1," +
                    "@add2,@tumbol,@p_code,@tel,@low_class,@top_clsass,@lat,@lng,@VatId)" +
                    "ON DUPLICATE KEY UPDATE (" +
                    "`smis`=@smis,`sc_code`=@sc_code,`sc_name`=@sc_name,`areacode`=@areacode,`sc_network`=@sc_network,`aumphur`=@aumphur,`province`=@province,`spt`=@spt,`add1`=@add1" +
                    ",`add2`=@add2,`tumbol`=@tumbol,`p_code`=@p_code,`tel`=@tel,`low_class`=@low_class,`top_clsass`=@top_clsass,`lat`=@lat,`lng`=@lng,`VatId`=@VatId)", conn);

                cmd.Parameters.AddWithValue("@sc_id", data.sc_id);
                cmd.Parameters.AddWithValue("@smis", data.smis);
                cmd.Parameters.AddWithValue("@sc_code", data.sc_code);
                cmd.Parameters.AddWithValue("@sc_name", data.sc_name);
                cmd.Parameters.AddWithValue("@areacode", data.areacode);
                cmd.Parameters.AddWithValue("@sc_network", data.sc_network);
                cmd.Parameters.AddWithValue("@aumphur", data.aumphur);
                cmd.Parameters.AddWithValue("@province", data.province);
                cmd.Parameters.AddWithValue("@spt", data.spt);
                cmd.Parameters.AddWithValue("@add1", data.add1);
                cmd.Parameters.AddWithValue("@add2", data.add2);
                cmd.Parameters.AddWithValue("@tumbol", data.tumbol);
                cmd.Parameters.AddWithValue("@p_code", data.p_code);
                cmd.Parameters.AddWithValue("@tel", data.tel);
                cmd.Parameters.AddWithValue("@low_class", data.low_class);
                cmd.Parameters.AddWithValue("@top_clsass", data.top_class);
                cmd.Parameters.AddWithValue("@lat", data.lat);
                cmd.Parameters.AddWithValue("@lng", data.lng);
                cmd.Parameters.AddWithValue("@VatId", data.VatId);

                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM `School` WHERE `sc_id`=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        private SchoolData ReadData(MySqlDataReader reader)
        {
            return new SchoolData()
            {
                sc_id = Convert.ToInt32(reader["sc_id"]),
                smis = Convert.ToInt32(reader["smis"]),
                sc_code = Convert.ToInt32(reader["sc_code"]),
                sc_name = reader["sc_name"].ToString(),
                areacode = reader["areacode"].ToString(),
                sc_network = Convert.ToInt32(reader["sc_network"]),
                aumphur = reader["aumphur"].ToString(),
                province = reader["province"].ToString(),
                spt = reader["spt"].ToString(),
                add1 = reader["add1"].ToString(),
                add2 = reader["add2"].ToString(),
                tumbol = reader["tumbol"].ToString(),
                p_code = Convert.ToInt32(reader["p_code"]),
                tel = reader["tel"].ToString(),
                low_class = reader["low_class"].ToString(),
                top_class = reader["top_clsass"].ToString(),
                lat = reader["lat"].ToString(),
                lng = reader["lng"].ToString(),
                VatId = reader["VatId"].ToString(),
            };
        }
    }
}
