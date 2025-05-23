# Cloud Computing
**Cloud computing** is a model for delivering computing services over the internet. Computing services include virtual machines, storage, databases, networking, and etc.
## Shared Responsibility Model
With the **shared responsibility model**, the responsibilities:
- physical data-center
- physical network
- physical hosts
- operating systems
- network controls
and etc. get shared between the cloud provider and the consumer.

With an on-premises data-center, the consumer (organization) is responsible for everything. The shared responsibility model is heavily tied into the cloud service types: IaaS, PaaS, and SaaS.
## Cloud Models
**Cloud models** define the deployment type of cloud resources.

**Private cloud** is used by a single entity. It provides much greater control for the organization and its IT department.
- Private clouds can be hosted from organizations onsite data-center.
- Private clouds can also run offsite in a dedicated data-center - possibly by one a third party reserves just for the organization.

**Public cloud** is built, controlled and maintained by a cloud provider. Organizations wanted to purchase cloud services can access and use resources.

**Hybrid cloud** uses both private and public clouds. It provides flexibility with providing additional layer of security.

| Public cloud                                 | Private cloud                                                 | Hybrid cloud                                                 |
| -------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------ |
| No CapEx                                     | Initial CapEx                                                 | Partial CapEx (for private portion)                          |
| No full control over security & compliance   | Full control over security & meet any compliance requirements | Flexible control (full control over sensitive workloads)     |
| No further maintenance & IT skills/expertise | Require further maintenance & IT skills/expertise             | Partial maintenance requirements (based on deployment model) |
| Pay only for what they use                   | Resources must be purchased for startup                       | Mix of pay-per-use and fixed costs                           |
In a **multi-cloud** scenario, organizations use multiple cloud providers. Organizations
- maybe using different features from different providers
- started with one provider and are in the process of migrating to a different provider

**Azure arc** can help organizations manage their cloud environment in:
- public cloud entirely on Azure
- private cloud in their data-center
- hybrid cloud
- multi-cloud with different cloud providers

**Azure VMware solution** lets organizations run their VMware workloads in Azure with seamless integration and scalalability.
## Consumption-based model
**Capital Expenditure** (CapEx) is a one-time, up-front expenditure to purchase or secure resources. Examples: a new building, building a data-center, and etc.

**Operational Expenditure** (OpEx) is spending money on services or products over time. Examples: renting a building, leasing a vehicle, signing up for cloud services, and etc.

Cloud computing falls under OpEx and operates on a **consumption-based model**. Organizations don't pay for up-front, data-center maintenance, only pay for the resources they use.

Cons of traditional data-center:
- with overestimating spending more on resources and waste money
- with underestimating decreased performance and a long-time fixing

Pros of cloud computing:
- adding and removing resources in any time
- pay only for what used, not "extra capacity"

**Pay-as-you-go** pricing model helps organizations:
- plan and manage their operating costs
- run infrastructure more efficiently
- scale dynamically as needed