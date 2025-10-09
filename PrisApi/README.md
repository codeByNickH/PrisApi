# PrisApi - Swedish Grocery Price Scraper

> **📚 Educational Project**  
> This project was created for learning purposes to demonstrate web scraping, .NET development, and database design skills. It is not intended for commercial use or production deployment. See [Compliance Status](#️-compliance-status) for details.

A comprehensive ASP.NET Core Web API demonstrating web scraping, browser automation, and database design skills through a grocery price tracking system.

## 🎯 What It Does

Automates collection of product pricing data from five Swedish grocery retailers (ICA, Willys, Coop, City Gross, Hemköp), handling complex pricing scenarios like multi-buy discounts, member prices, and unit conversions.

## 🛠️ Technical Stack

- **Backend**: ASP.NET Core 8, Entity Framework Core 9, SQL Server
- **Web Scraping**: Microsoft Playwright (headless browser automation)
- **Architecture**: Repository Pattern, Dependency Injection, Service Layer
- **Data Processing**: JSON parsing, regex, unit normalization (g→kg, ml→l)

## 🏗️ Key Technical Features

**Dynamic Configuration System**
- Database-stored scraper configurations (no hardcoded selectors)
- Easy adaptation to website changes without redeployment

**Complex Price Parsing**
```csharp
// Handles: "3 för 50kr", member pricing, quantity limits
// Normalizes units across different formats
// Tracks price history for trend analysis
```

**Intelligent Data Processing**
- Smart product matching to avoid duplicates
- Handles 10+ pricing scenarios (multi-buy, member-only, limits)
- Unit conversions and price normalization

**Database Design**
- Products with price history (time-series tracking)
- Many-to-many category relationships
- Store locations and configurations
- Scraping job metadata

## 📊 Technical Specs

- 90 products per category (limited scope testing)
- 18 product categories × 5 stores
- 25+ store locations across Sweden
- 10-second request delays (respectful automation)

## 💡 What I Learned (The Honest Version)

**Initial Approach**: Built this to learn web scraping and .NET. Like many beginners, I dove straight into implementation and tested against live sites.

**The Learning Moment**: Later discovered I should have checked robots.txt and Terms of Service *first*. This taught me a crucial lesson about responsible automation.

**Improvements Made**:
- ✅ Increased delays from 50ms → 10 seconds (200x improvement)
- ✅ Limited scope from comprehensive scraping → 90 products/category
- ✅ Researched robots.txt compliance for all target sites
- ✅ Repositioned as educational project only

**Key Takeaway**: Technical capability must be balanced with legal and ethical responsibility. I now research compliance requirements before writing the first line of code for any automation project.

## 🎓 Skills Demonstrated

- **Backend Development**: RESTful API design, async/await patterns
- **Database Design**: Complex relationships, EF Core migrations
- **Web Scraping**: Browser automation, API interception, dynamic content handling
- **Data Processing**: String parsing, unit conversion, duplicate detection
- **Software Architecture**: SOLID principles, separation of concerns
- **Problem-Solving**: Adapting approach based on real-world constraints
- **Professional Judgment**: Understanding when and how to build responsibly

## ⚖️ Compliance Status

**Current Implementation**:
- robots.txt compliant (10-second delays, verified allowed paths)
- Limited educational testing (90 products/category)
- No commercial use or data resale
- Not deployed to production

**What This Would Need for Production**:
- Explicit written permission from each retailer
- Official API access or data partnership agreements
- Commercial licensing arrangements

## 💼 Why This Makes Me a Strong Candidate

**Technical Skills**: Built a complex, multi-layered system demonstrating proficiency across the .NET ecosystem.

**Learning & Growth**: Recognized compliance issues independently and made significant improvements without being told.

**Professional Maturity**: Understand the difference between technical capability and ethical responsibility.

**Problem-Solving**: Can articulate trade-offs, identify issues, and implement solutions.

---

**Development Note**: Like many developers learning web scraping, I initially tested this without fully understanding compliance requirements. After researching robots.txt and ToS, I made significant improvements to rate limiting and scope. This experience taught me to consider legal/ethical requirements from day one of any project - a lesson that makes me a more thoughtful developer.

*Status: Educational portfolio piece demonstrating technical skills and professional growth.*