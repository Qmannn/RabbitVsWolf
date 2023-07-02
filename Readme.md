## RabbitMq with MassTransit concurrency limiting

Solution created for testing and demonstration how to MassTransit with RabbitMq do concurenct message processing.

In this solution used CPU-independent event consumers (by design).

This case show that single process can consume really many events.
Example of usage for this solution is proxy event processing by HTTP/gRPS to another workers behind some NLB (like nginx/haproxy).

Conclusions:

1) When consumers wait with ThreadSleep - processing is too long because per each event system makes new thread. It is very slow.
2) Slow conslumers that not use 'async/await' and block process may exhaust ConcurrentMessageLimit or PrefetchCount. This leads to block execution of fast events - they not receive time slot for execution.


# How to run

1) Run docker-compose into folder RabbitMqDeployment with command `docker-compose up -d`
2) Run solution into VS/Rider
