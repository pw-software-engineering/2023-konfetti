import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/seat.dart';

class Ticket {
  String id;
  Event event;
  int priceInSmallestUnit;
  String sectorName;
  List<Seat> seats;

  Ticket(this.id, this.event, this.priceInSmallestUnit, this.sectorName,
      this.seats);

  factory Ticket.fromJson(Map<String, dynamic> json) {
    List<Seat> seats = _getSeatsFromJson(json);
    return Ticket(
      json['id'],
      Event.fromJson(json['event']),
      json['priceInSmallestUnit'],
      json['sectorName'],
      seats,
    );
  }

  static List<Seat> _getSeatsFromJson(Map<String, dynamic> json) {
    List<Seat> sectors = List.empty(growable: true);
    for (var s in json['seats']) {
      sectors.add(Seat.fromJson(s));
    }
    return sectors;
  }

  Map<String, dynamic> toJson() => {
        'id': id,
        'event': event.toJson(),
        'priceInSmallestUnit': priceInSmallestUnit,
        'sectorName': sectorName,
        'seats': seats.map((item) => item.toJson()).toList(),
      };
}
