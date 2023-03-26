enum ResponseCode {
  allGood(200),
  notFound(400),
  noResponseCode(null);

  const ResponseCode(this.value);
  final int? value;

  static ResponseCode getByCode(int? i) {
    return ResponseCode.values.firstWhere((x) => x.value == i);
  }

  String getResponseString() {
    switch (value) {
      case 200:
        return "Everything ok.";
      case 400:
        return "Endpoint not found";
      case null:
      default:
        return "Response code cannot be converted";
    }
  }
}
