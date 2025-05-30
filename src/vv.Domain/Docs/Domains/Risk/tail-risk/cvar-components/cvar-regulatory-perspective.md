# CVaR: Regulatory Perspective

> Regulatory requirements and applications of Conditional Value-at-Risk

---

## Basel Framework

* **Basel III/IV**: Shifted from VaR to Expected Shortfall (CVaR)
* **Confidence Level**: 97.5% for market risk
* **Time Horizon**: 10-day for market risk
* **Stressed Calibration**: Requires calibration to period of significant financial stress
* **Liquidity Horizons**: Different horizons for different risk factors

## Motivation for Regulatory Adoption

* **Coherence**: Mathematical property missing from VaR
* **Tail Sensitivity**: Better capture of extreme loss scenarios
* **Crisis Lessons**: Financial crisis revealed limitations of VaR
* **Aggregation Properties**: Better properties for risk aggregation
* **Systemic Risk**: Better suited for system-wide risk assessment

## Implementation Timeline

* **2016**: Introduction in Basel Committee's FRTB framework
* **2019**: Finalization of rules
* **2023+**: Phased implementation across jurisdictions
* **Transition Period**: Many institutions running both VaR and CVaR in parallel

## Other Regulatory Frameworks

### Solvency II (Insurance)

* **Risk Measure**: Uses VaR at 99.5% confidence over 1-year, but CVaR concepts influence internal models
* **Application**: Capital requirements for insurance companies
* **Implementation**: Through internal models or standard formula

### IFRS 9/17

* **Expected Loss Model**: Incorporates concepts similar to CVaR
* **Time Horizons**: Lifetime expected loss considerations
* **Application**: Accounting for financial instruments and insurance contracts

### Investment Fund Regulations

* **UCITS/AIFMD**: Increasing focus on tail risk disclosure
* **Reporting Requirements**: Enhanced risk reporting frameworks
* **Investor Protection**: Focus on communicating extreme risks

## Compliance Considerations

* **Governance**: Clear roles and responsibilities for CVaR models
* **Documentation**: Comprehensive documentation of methodology and assumptions
* **Model Validation**: Independent validation of CVaR models
* **Reporting**: Regular reporting of CVaR metrics to regulators
* **Stress Testing**: Integration with broader stress testing framework

## Related Documents

* [Conditional Value-at-Risk Overview](../conditional-value-at-risk.md)
* [CVaR Practical Considerations](./cvar-practical-considerations.md)
* [CVaR Advantages and Limitations](./cvar-advantages-limitations.md)