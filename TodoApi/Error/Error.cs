namespace TodoApi.Error {
    public class ErrorMessage {
        public string Message { get; private set; }

        public ErrorMessage(string message) {
            this.Message = message;
        }
    }

    public class Error : ErrorMessage {
        public int StatusCode { get; private set; }

        public string StatusDescription { get; private set; }

        public Error(int statusCode, string message) :
            base(message) {

            this.StatusCode = statusCode;
            this.StatusDescription = message;
        }
    }
}
