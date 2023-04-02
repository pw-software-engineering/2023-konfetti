import 'package:ticketer/backend_communication/model/sector.dart';

class Event {
  String name;
  String description;
  String location;
  String date;
  List<Sector> sectors;

  Event(
    this.name,
    this.description,
    this.location,
    this.date,
    this.sectors,
  );

  factory Event.fromJson(Map<String, dynamic> json) {
    return Event(
      json['name'],
      json['description'],
      json['location'],
      json['date'],
      json['sectors'].map((s) => Sector.fromJson(s)).toList(),
    );
  }

  Map<String, dynamic> toJson() => {
        'name': name,
        'description': description,
        'location': location,
        'date': date,
        'sectors': sectors.map((item) => item.toJson()).toList(),
      };
}
