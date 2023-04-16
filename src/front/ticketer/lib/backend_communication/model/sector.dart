class Sector {
  String name;
  double price;
  int numberOfRows;
  int numberOfColumns;

  Sector(
    this.name,
    this.price,
    this.numberOfRows,
    this.numberOfColumns,
  );

  factory Sector.fromJson(Map<String, dynamic> json) {
    return Sector(
      json['name'],
      json['priceInSmallestUnit'] / 100,
      json['numberOfRows'],
      json['numberOfColumns'],
    );
  }

  Map<String, dynamic> toJson() => {
        'name': name,
        'priceInSmallestUnit': (price * 100).round(),
        'numberOfColumns': numberOfColumns,
        'numberOfRows': numberOfRows,
      };

  @override
  String toString() {
    return '$name ($numberOfRows \u00d7 $numberOfColumns) - \$$price';
  }
}
