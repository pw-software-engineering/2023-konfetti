enum ResponseCode {
  allGood(200),
  notFound(404),
  unauthorisedAccess(401),
  badRequest(400),
  noResponseCode(-1);

  const ResponseCode(this.value);
  final int? value;

  static ResponseCode getByCode(int i) =>
    ResponseCode.values.firstWhere((x) => x.value == i);


  String getResponseString() {
    switch (value) {
      case 200:
        return "Everything ok.";
      case 404:
        return "Endpoint not found";
      case 401:
        return "Credentials are wrong for this action";
      case 400:
        return "Bad Request - Server cannot or will not process the request";
      case -1:
      default:
        return "Response code cannot be converted";
    }
  }
}
