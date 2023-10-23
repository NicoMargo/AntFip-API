using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IT_Arg_API.Models
{
    public class Certificate
    {
        private int? _certificateId;
        private DateOnly? _emission;
        private List<string>? _items;
        private string? _studentName;
        private string? _courseName;
        private int? _moduleId;
        private bool? _isMention;
        private int? _studentId;
        private string? _studentSurname;
        private string? _moduleName;
        private string? _code;



        //Create certificate
        public Certificate(string courseName, string moduleName)
        {
            _courseName = courseName;
            _moduleName = moduleName;
            _items = new List<string>();
        }

        public Certificate()
        {

        }


        public int? ModuleId { get => _moduleId; set => _moduleId = value; }
        public bool? IsMention { get => _isMention; set => _isMention = value; }
        public string? ModuleName { get => _moduleName; set => _moduleName = value; }
        public int? StudentId { get => _studentId; set => _studentId = value; }
        public string? Code { get => _code; set => _code = value; }
        public string? StudentSurname { get => _studentSurname; set => _studentSurname = value; }
        public int? CertificateId { get => _certificateId; set => _certificateId = value; }
        public DateOnly? Emission { get => _emission; set => _emission = value; }
        public List<string>? Items { get => _items; set => _items = value; }
        public string? StudentName { get => _studentName; set => _studentName = value; }
        public string? CourseName { get => _courseName; set => _courseName = value; }

        public bool certificateCreate(List<Certificate> listCertificates)
        {
            
                //All the student has the same Module Id
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pModuleId",listCertificates[0].ModuleId}
                };

                //Get the certificate data
                using JsonDocument doc = JsonDocument.Parse(DBHelper.callProcedureReader("spModuleGetById", args));
                JsonElement root = doc.RootElement;

                //Create the object with all the data of the certificate
                Certificate dataCertificate = new Certificate(
                    Convert.ToString(root[0].GetProperty("courseName")),
                    Convert.ToString(root[0].GetProperty("moduleName")));

                foreach (JsonElement description in root[1].EnumerateArray())
                    dataCertificate.Items.Add(Convert.ToString(description.GetProperty("description")));


                //Create each certificate
                Certificate auxCertificate = new Certificate();

                foreach (Certificate certificate in listCertificates)
                {
                    Dictionary<string, object> args2 = new Dictionary<string, object> {
                        {"pIdStudent",certificate.StudentId},
                        {"pIdModule",certificate.ModuleId},
                        {"pIsMention",certificate.IsMention}
                    };

                //Create the certificate in database and get data                
                auxCertificate = JsonSerializer.Deserialize<Certificate>(DBHelper.callProcedureReader("spCertificateCreate", args2));

                //Create the certificate img
                CertificateDraw.drawCertificate(auxCertificate.StudentName, auxCertificate.StudentSurname, dataCertificate.CourseName, dataCertificate.ModuleName, Convert.ToString(auxCertificate.Emission), auxCertificate.Code, dataCertificate.Items);

                //if the student has a mention, then it will be created
                if (certificate.IsMention == true)
                {
                    CertificateDraw.drawMention(auxCertificate.StudentName, auxCertificate.StudentSurname, dataCertificate.CourseName, dataCertificate.ModuleName, Convert.ToString(auxCertificate.Emission), auxCertificate.Code);
                }

                };

                return true;           

        }
        
    }
}
