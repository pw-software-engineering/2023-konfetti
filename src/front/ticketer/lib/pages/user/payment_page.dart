import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/pages/common/app_bar.dart';

class PaymentPage extends StatefulWidget {
  final Event event;
  final List<int> seatsInSectors;

  const PaymentPage(
      {Key? key, required this.event, required this.seatsInSectors})
      : super(key: key);

  @override
  State<PaymentPage> createState() => _PaymentPageState();
}

class _PaymentPageState extends State<PaymentPage> {
  late Event _event;
  late List<int> _seatsInSectors;
  late List<String> _paymentIds = List.empty(growable: true);

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getGreeting(),
          Container(
            padding: const EdgeInsets.all(10.0),
            decoration: BoxDecoration(
              border: Border.all(
                color: Theme.of(context).hintColor,
              ),
            ),
            child: Column(
              children: [
                _getEventInfo("${_event.name}, ${_event.date}"),
                _getSectorList(),
                _getSummary(),
              ],
            ),
          ),
          _getButtons(),
        ],
      ),
    );
  }

  Container _getGreeting() {
    return Container(
      margin: const EdgeInsets.only(bottom: 25.0),
      child: Text(
        "Review your purchase",
        style: TextStyle(
          fontSize: 24,
          color: Theme.of(context).primaryColor,
        ),
      ),
    );
  }

  Widget _getEventInfo(String text) {
    return Container(
      margin: const EdgeInsets.only(bottom: 18.0),
      child: Text(
        text,
        style: const TextStyle(
          fontSize: 18,
          fontWeight: FontWeight.w600,
        ),
      ),
    );
  }

  Widget _getSummary() {
    double total = 0.0;
    for (int i = 0; i < _seatsInSectors.length; i++) {
      total += _seatsInSectors[i] * _event.sectors[i].price;
    }
    return Container(
      padding: const EdgeInsets.only(
        top: 10.0,
        bottom: 10.0,
        right: 30.0,
      ),
      child: Row(mainAxisAlignment: MainAxisAlignment.end, children: [
        Text(
          "TOTAL: ",
          style: TextStyle(
            color: Theme.of(context).primaryColor,
            fontSize: 16,
            fontWeight: FontWeight.w600,
          ),
        ),
        Text(
          "\$${double.parse(total.toStringAsFixed(2)).toStringAsFixed(2)}",
          style: const TextStyle(
            fontSize: 16,
            fontWeight: FontWeight.w600,
          ),
        )
      ]),
    );
  }

  Widget _getButtons() {
    return Container(
      padding: const EdgeInsets.only(top: 10.0),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          ElevatedButton(
            onPressed: () => {
              Navigator.pop(context),
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.redAccent,
              fixedSize: const Size(100, 35),
            ),
            child: const Text('Cancel'),
          ),
          Padding(
            padding: const EdgeInsets.only(left: 8.0),
            child: ElevatedButton(
              onPressed: () async {
                // Navigator.pop(context),
                // todo: handle returned ids into ticketsIds list
                // and continue with logic:
                for (int i = 0; i < _seatsInSectors.length; ++i) {
                  if (_seatsInSectors[i] > 0) {
                    var response = await BackendCommunication()
                        .ticket
                        .buy(_event, _event.sectors[i], _seatsInSectors[i]);
                    if (response.item2.value == 200) {
                      String id = response.item1.data["paymentId"];
                      _paymentIds.add(id);
                    }
                  }
                }
                // proceed to payment
                for (var id in _paymentIds) {
                  await PaymentCommunication().payment.confirm(id);
                  var response =
                      await PaymentCommunication().payment.finish(id);
                  if (response.item2.value == 200) {
                    // ignore: use_build_context_synchronously
                    await showDialog(
                      context: context,
                      builder: (context) {
                        return _showDialogOnSuccess(context);
                      },
                    );
                  }
                }
              },
              style: ElevatedButton.styleFrom(
                fixedSize: const Size(100, 35),
              ),
              child: const Text('Purchase'),
            ),
          ),
        ],
      ),
    );
  }

  AlertDialog _showDialogOnSuccess(BuildContext context) {
    return AlertDialog(
      title: const Text(
        "Payment confirmed!",
        style: TextStyle(
          fontSize: 20,
          fontWeight: FontWeight.w600,
        ),
      ),
      scrollable: true,
      content: SingleChildScrollView(
        child: SizedBox(
            width: MediaQuery.of(context).size.width / 4,
            height: MediaQuery.of(context).size.height / 2,
            child: const Text("Your payment has been confirmed successfully!")),
      ),
      actions: [
        ElevatedButton(
          onPressed: () => {
            Navigator.of(context).popUntil((route) => route.isFirst),
          },
          child: const Text('OK'),
        ),
      ],
    );
  }

  SingleChildScrollView _getSectorList() {
    List<int> sectorIndexes = [];
    for (int i = 0; i < _seatsInSectors.length; i++) {
      if (_seatsInSectors[i] > 0) sectorIndexes.add(i);
    }
    return SingleChildScrollView(child: _getSectorsTable(sectorIndexes));
  }

  DataTable _getSectorsTable(List<int> sectorIndexes) {
    return DataTable(
      columns: <DataColumn>[
        const DataColumn(
          label: Expanded(
            child: Text(
              'Sector name',
              style: TextStyle(fontWeight: FontWeight.w600),
            ),
          ),
        ),
        const DataColumn(
          label: Expanded(
            child: Text(
              'Qty',
              style: TextStyle(fontWeight: FontWeight.w600),
            ),
          ),
        ),
        const DataColumn(
          label: Expanded(
            child: Text(
              'Price / pc',
              style: TextStyle(fontWeight: FontWeight.w600),
            ),
          ),
        ),
        DataColumn(
          label: Expanded(
            child: Text(
              'Price total',
              style: TextStyle(
                  color: Theme.of(context).primaryColor,
                  fontWeight: FontWeight.w600),
            ),
          ),
        ),
      ],
      rows: sectorIndexes.map((e) => _toDataRow(e)).toList(),
    );
  }

  DataRow _toDataRow(int index) {
    return DataRow(
      cells: <DataCell>[
        DataCell(
          Text(_event.sectors[index].name),
        ),
        DataCell(
          Text(_seatsInSectors[index].toString()),
        ),
        DataCell(
          Text("\$${_event.sectors[index].price}"),
        ),
        DataCell(
          Text(_calculateSectorPrice(index)),
        ),
      ],
    );
  }

  String _calculateSectorPrice(int index) {
    return "\$${double.parse((_seatsInSectors[index] * _event.sectors[index].price).toStringAsFixed(2)).toStringAsFixed(2)}";
  }

  @override
  void initState() {
    _event = widget.event;
    _seatsInSectors = widget.seatsInSectors;
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: ticketerAppBar("Confirm your purchase"),
      body: Center(
        child: Container(
          constraints: const BoxConstraints(minWidth: 200, maxWidth: 600),
          padding: const EdgeInsets.all(20),
          margin: const EdgeInsets.all(8),
          child: _getContent(),
        ),
      ),
    );
  }
}
