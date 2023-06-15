class Seat {
  int row;
  int column;

  Seat(
    this.row,
    this.column,
  );

  factory Seat.fromJson(Map<String, dynamic> json) {
    return Seat(
      json['row'],
      json['column'],
    );
  }

  Map<String, dynamic> toJson() => {
        'row': row,
        'column': column,
      };
}
