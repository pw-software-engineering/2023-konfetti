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
      json['priceInSmallestUnit'],
      json['numberOfRows'],
      json['numberOfColumns'],
    );
  }

  Map<String, dynamic> toJson() => {
        'name': name,
        'priceInSmallestUnit': price,
        'numberOfColumns': numberOfColumns,
        'numberOfRows': numberOfRows,
      };

  @override
  String toString() {
    return '$name ($numberOfRows \u00d7 $numberOfColumns) - \$$price';
  }
}
