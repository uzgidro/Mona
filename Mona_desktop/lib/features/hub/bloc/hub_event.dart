part of 'hub_bloc.dart';

@immutable
sealed class HubEvent {}

final class StartConnection extends HubEvent {}
