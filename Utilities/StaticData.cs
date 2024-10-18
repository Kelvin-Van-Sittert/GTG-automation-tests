using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTG_automation_tests.Utilities
{
    public static class StaticData {
        //Dev box URLs
        public const string CETE_01_URL = "https://dev.cardeasexml.com:44300/generic.cex?be=uk1-odt-cete-01";
        public const string CETE_02_URL = "https://dev.cardeasexml.com:44300/generic.cex?be=uk1-odt-cete-02";

        //Log file path
        //Down the line we can update this to be in dictated by the dev box URL
        public const string A70_LOG_PATH = "//uk1-odt-cete-01.creditcall.co.uk/c$/CardEaseV2/Logs/A70Server/A70Server.log";
        public const string ELAVON8583_PROCESSOR_LOG_PATH = "//uk1-odt-cete-01.creditcall.co.uk/c$/CardEaseV2/Logs/CardEaseV2Elavon8583ProcessorHttpServer/CardEaseV2Elavon8583Processor.HttpServer.log";
        public const string ELAVON8583_SETTLER_LOG_PATH = "//uk1-odt-cete-01.creditcall.co.uk/c$/CardEaseV2/Logs/CardEaseV2Elavon8583Settler/CardEaseV2Elavon8583Settler.log";
        public const string TSYS_PROCESSOR_LOG_PATH = "//uk1-odt-cete-01.creditcall.co.uk/c$/CardEaseV2/Logs/CardEaseV2TsysSierraEmvProcessorHttpServer/CardEaseV2TsysSierraEmvProcessor.HttpServer.log";
        public const string TSYS_SETTLER_LOG_PATH = "//uk1-odt-cete-01.creditcall.co.uk/c$/CardEaseV2/Logs/CardEaseV2Elavon8583Settler/CardEaseV2Elavon8583Settler.log";

        //Get a list of merchants and the corrosponding acquirer for each one
        //E.g. When the merchant is relavent to Tsys we should look in the Tsys logs
        //The merchant is defined in the xml request payload - with a string

        //We will need to access the specific log
        //Tysy settler/processer log, elevon settler/processer log, A70 server, A70 settler


        //IDs
        public const string  TID = "99940407";
        public const string  KEY = "2709";
        public const string  MID = "6818934";
        public const string  BARC_TID1 = "90012220";

        // ECI value for Ecomm (Suffixes for Visa, Mastercard, Discover, Amex)
        public const string  ECI_FAILED_M = "00";
        public const string  ECI_FAILED_VAD = "07";
        public const string  ECI_SUCCESS_M = "02";
        public const string  ECI_SUCCESS_VAD = "05";
        public const string  ECI_ATTEMPTED_M = "01";
        public const string  ECI_ATTEMPTED_VAD = "06";

        // 3DS Major/Minor version. 
        public const string  ThreeDSProtocolVersion = "2.2.0";

        //XML Payloads
        public const string xmlPayloadForTesting3DSWithMasterCard = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
                <Request type=""CardEaseXML"" version=""1.5.0"">
                    <TransactionDetails>
                        <MessageType autoconfirm=""true"">Auth</MessageType>
                        <Amount>3331</Amount>
                        <CurrencyCode>840</CurrencyCode>
                    </TransactionDetails>
                    <TerminalDetails>
                        <TerminalID>99962964</TerminalID>
                        <TransactionKey>4614</TransactionKey>
                    </TerminalDetails>
                    <CardDetails>
                        <Manual type=""mailorder"">
                            <PAN>4012000098765439</PAN>
                            <ExpiryDate format=""yyMM"">2512</ExpiryDate>
                        </Manual>
                        <AdditionalVerification>
                            <CSC>999</CSC>
                        </AdditionalVerification>
                    </CardDetails>
                </Request>";
    }
}
