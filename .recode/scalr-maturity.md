# ScalR maturity report — C:\Users\mathew.burkitt\source\repos\DT\tigerbeetle

**Gate Stage:** Garage/PoC  
**Breadth Score:** 66

| Row | Gate | Stage | Evidence |
|---|---|---|---|
| Identity | Mandatory | Prototype | 3 partial identity signals found — real but incomplete |
| Authorization | Mandatory | Small team | role/guard file path found |
| Organization model | Mandatory | Small team | organization/workspace reference found |
| Security | Mandatory | Garage/PoC | a single partial security signal found — crude attempt |
| Compliance / Privacy | Mandatory | insufficient evidence | no privacy/consent evidence found in-repo |
| Auditability | Mandatory | Enterprise | event-sourced/append-only audit store reference found |
| Reliability | Mandatory | Corporate | retry/circuit-breaker dep or backup config found |
| Scale | Supplementary | Enterprise | sharding/rate-limit code found |
| Data management | Mandatory | Prototype | 3 partial data management signals found — real but incomplete |
| Integration | Supplementary | Enterprise | event-stream producer dep or quota middleware found |
| Administration | Mandatory | Small team | admin route/page found |
| Deployment | Supplementary | Enterprise | canary/VPC/dedicated-tenancy config found |
| Observability | Supplementary | Prototype | 3 partial observability signals found — real but incomplete |
| Change management | Mandatory | Enterprise | deprecation-window code or compat tests found |
| Testing | Supplementary | Enterprise | perf/load or chaos test scripts found |
| Documentation | Supplementary | Small team | docs/ folder found |
| Accessibility | Supplementary | Garage/PoC | a single partial accessibility signal found — crude attempt |
| Internationalization | Supplementary | Prototype | 2 partial internationalization signals found — real but incomplete |
| Customization | Supplementary | Enterprise | plugin/extension-point architecture found |
| Lifecycle / Ops ownership | Supplementary | insufficient evidence | no ownership/on-call evidence found in-repo |
| Acceptable tech debt | Supplementary | Ideation | low TODO/FIXME density (3/157 files), no ticket tracking found |
| Product design | Supplementary | n/a — manual attestation | org/contract fact — not code-detectable |
| Support | Mandatory | n/a — manual attestation | org/contract fact — not code-detectable |
| Procurement | Supplementary | n/a — manual attestation | org/contract fact — not code-detectable |

## Requires manual attestation (not code-detectable)
- Product design
- Support
- Procurement

## Insufficient evidence (no IaC/config found — not necessarily absent)
- Compliance / Privacy
- Lifecycle / Ops ownership

