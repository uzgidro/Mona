import 'package:flutter/material.dart';

class SideMenu extends StatefulWidget {
  @override
  State<SideMenu> createState() => _SideMenuState();
}

class _SideMenuState extends State<SideMenu> {
  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.of(context).size.height,
      color: Theme.of(context).colorScheme.secondary,
      child: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Builder(builder: (context) {
                return SizedBox.square(
                  child: IconButton(
                    onPressed: () => Scaffold.of(context).openDrawer(),
                    icon: Icon(Icons.menu,
                        color: Theme.of(context).colorScheme.background),
                  ),
                );
              }),
            ),
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Icon(Icons.forum_rounded,
                  color: Theme.of(context).colorScheme.background),
            ),
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Icon(Icons.group_rounded,
                  color: Theme.of(context).colorScheme.background),
            ),
          ],
        ),
      ),
    );
  }
}
