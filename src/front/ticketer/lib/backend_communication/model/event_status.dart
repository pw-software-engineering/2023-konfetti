enum EventStatus {
  Unverified(0),
  Verified(1),
  Published(2),
  Opened(3),
  Closed(4),
  Finished(5),
  Cancelled(6),
  Held(7),
  Recalled(8);

  const EventStatus(this.value);
  final int? value;

  static EventStatus getByCode(int i) {
  return EventStatus.values.firstWhere((x) => x.value == i);
  }

  String getStatusName() {
    switch (value) {
      case 0:
        return "Unverified";
      case 1:
        return "Verified";
      case 2:
        return "Published";
      case 3:
        return "Opened";
      case 4:
        return "Closed";
      case 5:
        return "Finished";
      case 6:
        return "Cancelled";
      case 7:
        return "Held";
      case 8:
        return "Recalled";
      default:
        return "Unknown status";
    }
  }
}
