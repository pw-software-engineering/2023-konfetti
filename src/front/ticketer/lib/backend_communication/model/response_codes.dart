enum ResponseCode {
  allGood(200),
  notFound(400),
  unauthorisedAccess(401),
  noResponseCode(-1);

  const ResponseCode(this.value);
  final int? value;

  static ResponseCode getByCode(int i) {
    return ResponseCode.values.firstWhere((x) => x.value == i);
  }

  String getResponseString() {
    switch (value) {
      case 200:
        return "Everything ok.";
      case 400:
        return "Endpoint not found";
      case 401:
        return "Credentials are wrong for this action";
      case -1:
      default:
        return "Response code cannot be converted";
    }
  }
}
