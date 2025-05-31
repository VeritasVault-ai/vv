---
document_type: guide
classification: internal
status: draft
version: 0.1.0
last_updated: '2025-05-31'
applies_to:
- Core
reviewers:
- '@tech-lead'
priority: p2
next_review: '2026-05-31'
---

# Social Media & Forum Data Sources

> Public conversation and investor discussion platforms utilized for market sentiment analysis

---

## Overview

This document details the social media platforms and investment forums integrated into VeritasVault's market sentiment analysis system. These sources capture retail investor sentiment, public perception of companies, and financial market discussions.

## Investment-Focused Social Platforms

### Investment Forums

* **Retail Investor Communities**
  * Reddit (r/wallstreetbets, r/investing, r/stocks)
  * StockTwits message boards
  * Yahoo Finance conversation threads
  * InvestorHub (iHub) discussions

* **Professional Trading Forums**
  * Elite Trader
  * Trading View comments
  * Forex Factory
  * Futures.io discussions

* **Broker-Hosted Communities**
  * Interactive Brokers community forum
  * TD Ameritrade thinkScript community
  * Robinhood Snacks community
  * Fidelity community discussions

### Financial Social Networks

* **Investment-Focused Platforms**
  * StockTwits
  * eToro social trading
  * TradingView social features
  * Commonstock discussions

* **Professional Networks**
  * LinkedIn financial content
  * Bloomberg Terminal MESSAGE
  * Symphony messaging
  * Finance industry Slack communities

* **Crowdsourced Research Platforms**
  * Seeking Alpha comments
  * Estimize contributor insights
  * SumZero investment ideas
  * Value Investors Club

## General Social Media Platforms

### Major Social Networks

* **Twitter/X Data**
  * Cashtag monitoring ($TICKER)
  * Financial influencer tracking
  * Financial news account analysis
  * Company and executive accounts

* **Reddit Communities**
  * Financial subreddit analysis
  * Company-specific subreddits
  * Industry-focused communities
  * Economic discussion forums

* **Other Network Data**
  * LinkedIn company mentions
  * Facebook financial groups
  * YouTube financial creator comments
  * TikTok financial content trends

### Discussion Platforms

* **Question & Answer Sites**
  * Quora financial topics
  * Stack Exchange Money & Personal Finance
  * Yahoo Finance Conversations
  * Specialized financial Q&A platforms

* **Comment Sections**
  * Financial news article comments
  * Financial blog discussions
  * YouTube financial video comments
  * Podcast episode discussions

## Data Collection Methodology

### Social Listening Techniques

* **Keyword and Cashtag Monitoring**
  * Company name variations
  * Ticker symbol tracking
  * Product and service mentions
  * Executive name monitoring

* **Entity-Relationship Mapping**
  * Supply chain relationship tracking
  * Competitor mention analysis
  * Partnership discussion monitoring
  * Regulatory relationship tracking

* **Event-Based Listening**
  * Earnings announcement reactions
  * Product launch discussions
  * M&A speculation monitoring
  * Crisis response tracking

### Volume and Engagement Metrics

* **Discussion Volume Analytics**
  * Mention frequency tracking
  * Conversation thread growth
  * Topic velocity measurement
  * Relative volume comparison

* **Engagement Analysis**
  * Like and share patterns
  * Comment depth analysis
  * Repost and quote behavior
  * Influential engagement tracking

* **Temporal Patterns**
  * Time-of-day distribution
  * Day-of-week patterns
  * Event-related volume spikes
  * Sustained conversation monitoring

## Sentiment Analysis Adaptations

### Platform-Specific Analysis

* **Twitter/X-Specific Processing**
  * Hashtag and cashtag handling
  * Thread reconstruction
  * Retweet vs. original content analysis
  * Quote tweet context incorporation

* **Reddit-Specific Analysis**
  * Subreddit context integration
  * Award and voting incorporation
  * Moderator and flair consideration
  * Thread structure analysis

* **StockTwits-Specific Processing**
  * Bullish/bearish indicator integration
  * Sentiment label incorporation
  * Trader classification consideration
  * Historical user sentiment tracking

### Social-Specific Challenges

* **Sarcasm and Irony Detection**
  * Contextual sarcasm identification
  * Community-specific humor patterns
  * Counter-sentiment markers
  * Emoji and symbol context

* **Slang and Jargon Processing**
  * Financial slang dictionaries
  * Community-specific terminology
  * Acronym expansion and handling
  * Emoji sentiment interpretation

* **Spam and Bot Detection**
  * Bot account identification
  * Coordinated posting patterns
  * Low-quality content filtering
  * Promotional content detection

## Implementation Example

```python
# Simplified example of social media sentiment analysis integration
import pandas as pd
from datetime import datetime, timedelta
import networkx as nx
import numpy as np

class SocialMediaSentimentAnalyzer:
    def __init__(self, platform_weights=None):
        # Default weights for different platforms
        self.platform_weights = platform_weights or {
            "stocktwits": 0.8,
            "twitter": 0.7,
            "reddit_wsb": 0.5,
            "reddit_investing": 0.75,
            "linkedin": 0.85,
            "yahoo_finance": 0.6,
            "tradingview": 0.7,
            "seeking_alpha_comments": 0.65,
            "other": 0.4
        }
        
        # Initialize data storage
        self.posts_data = pd.DataFrame()
        self.user_reputation = {}
        self.entity_mentions = nx.DiGraph()
        
    def add_post_data(self, platform, user_id, content, timestamp, entities, sentiment_score, 
                      engagement_metrics, post_url):
        """Add social media post data to the analyzer"""
        
        # Apply platform weight and adjust for user reputation
        platform_key = platform.lower() if platform.lower() in self.platform_weights else "other"
        user_rep = self.user_reputation.get(user_id, 0.5)
        
        # Calculate engagement score
        engagement_score = self._calculate_engagement_score(engagement_metrics, platform)
        
        # Adjust sentiment based on platform, user reputation and engagement
        weighted_sentiment = sentiment_score * self.platform_weights[platform_key] * (0.5 + (user_rep * 0.5))
        impact_score = weighted_sentiment * (0.7 + (engagement_score * 0.3))
        
        # Create new data entry
        new_data = pd.DataFrame({
            "platform": [platform],
            "user_id": [user_id],
            "content": [content],
            "timestamp": [timestamp],
            "raw_sentiment": [sentiment_score],
            "weighted_sentiment": [weighted_sentiment],
            "impact_score": [impact_score],
            "engagement_score": [engagement_score],
            "url": [post_url]
        })
        
        # Add to posts dataframe
        self.posts_data = pd.concat([self.posts_data, new_data], ignore_index=True)
        
        # Update entity mention graph
        for entity in entities:
            if entity not in self.entity_mentions:
                self.entity_mentions.add_node(entity, mentions=0, sentiment_sum=0)
            
            self.entity_mentions.nodes[entity]["mentions"] += 1
            self.entity_mentions.nodes[entity]["sentiment_sum"] += weighted_sentiment
            
            # Add connections between co-mentioned entities
            for other_entity in entities:
                if entity != other_entity:
                    if not self.entity_mentions.has_edge(entity, other_entity):
                        self.entity_mentions.add_edge(entity, other_entity, weight=0)
                    self.entity_mentions[entity][other_entity]["weight"] += 1
    
    def _calculate_engagement_score(self, metrics, platform):
        """Calculate normalized engagement score based on platform-specific metrics"""
        if platform.lower() == "twitter":
            # Twitter engagement formula
            return min(1.0, (metrics.get("likes", 0) * 1.0 + 
                          metrics.get("retweets", 0) * 2.0 + 
                          metrics.get("replies", 0) * 1.5) / 100)
        elif platform.lower() == "reddit":
            # Reddit engagement formula
            return min(1.0, (metrics.get("score", 0) * 1.0 + 
                          metrics.get("comments", 0) * 2.0) / 50)
        elif platform.lower() == "stocktwits":
            # StockTwits engagement formula
            return min(1.0, (metrics.get("likes", 0) * 1.0 + 
                          metrics.get("reshares", 0) * 1.5) / 30)
        else:
            # Generic engagement formula
            total_engagement = sum(metrics.values())
            return min(1.0, total_engagement / 50)
            
    def update_user_reputation(self, user_id, accuracy_score):
        """Update reputation score for a user based on prediction accuracy"""
        current = self.user_reputation.get(user_id, 0.5)
        # Exponential moving average with 0.1 learning rate
        self.user_reputation[user_id] = (current * 0.9) + (accuracy_score * 0.1)
        
    def get_entity_sentiment(self, entity, time_window=None):
        """Get social sentiment for a specific entity within optional time window"""
        # Filter for entity mentions
        if entity not in self.entity_mentions:
            return None
            
        entity_data = self.posts_data[self.posts_data["content"].str.contains(entity, case=False)]
        
        if time_window:
            # Apply time filter if specified
            cutoff_time = datetime.now() - time_window
            entity_data = entity_data[entity_data["timestamp"] >= cutoff_time]
        
        if entity_data.empty:
            return None
            
        # Calculate aggregated sentiment metrics
        result = {
            "entity": entity,
            "mention_count": len(entity_data),
            "platforms": entity_data["platform"].value_counts().to_dict(),
            "avg_raw_sentiment": entity_data["raw_sentiment"].mean(),
            "avg_weighted_sentiment": entity_data["weighted_sentiment"].mean(),
            "avg_impact_score": entity_data["impact_score"].mean(),
            "sentiment_std": entity_data["raw_sentiment"].std(),
            "earliest_mention": entity_data["timestamp"].min(),
            "latest_mention": entity_data["timestamp"].max(),
            "engagement_weighted_sentiment": (entity_data["weighted_sentiment"] * 
                                             entity_data["engagement_score"]).sum() / 
                                             entity_data["engagement_score"].sum()
        }
        
        # Find related entities (from entity graph)
        if entity in self.entity_mentions:
            related = sorted([(n, self.entity_mentions[entity][n]["weight"]) 
                           for n in self.entity_mentions.neighbors(entity)],
                           key=lambda x: x[1], reverse=True)
            result["related_entities"] = related[:5]
        
        return result
        
    def get_trending_topics(self, n=10, time_window=timedelta(hours=24)):
        """Get trending topics based on mention volume and sentiment shift"""
        cutoff_time = datetime.now() - time_window
        recent_data = self.posts_data[self.posts_data["timestamp"] >= cutoff_time]
        
        if recent_data.empty:
            return []
        
        # Use entity mention graph to find important entities
        trending = []
        for entity in self.entity_mentions.nodes():
            entity_data = recent_data[recent_data["content"].str.contains(entity, case=False)]
            if len(entity_data) < 5:  # Require minimum mention count
                continue
                
            # Calculate sentiment and volume metrics
            sentiment_mean = entity_data["weighted_sentiment"].mean()
            sentiment_stdev = entity_data["weighted_sentiment"].std()
            volume = len(entity_data)
            
            # Measure volume increase (recent vs previous period)
            previous_cutoff = cutoff_time - time_window
            previous_data = self.posts_data[(self.posts_data["timestamp"] >= previous_cutoff) & 
                                         (self.posts_data["timestamp"] < cutoff_time)]
            previous_mentions = sum(previous_data["content"].str.contains(entity, case=False))
            
            volume_change = ((volume - previous_mentions) / max(1, previous_mentions)) * 100
            
            # Calculate trending score
            trend_score = (volume * 0.4) + (abs(sentiment_mean) * 30 * 0.3) + (volume_change * 0.3)
            
            trending.append({
                "entity": entity,
                "mentions": volume,
                "sentiment": sentiment_mean,
                "sentiment_stdev": sentiment_stdev,
                "volume_change_pct": volume_change,
                "trend_score": trend_score
            })
            
        # Return top N trending entities
        return sorted(trending, key=lambda x: x["trend_score"], reverse=True)[:n]
```

## User and Source Evaluation

### User Credibility Framework

* **Influence Metrics**
  * Follower and connection counts
  * Engagement rate assessment
  * Content reach measurement
  * Citation and reference tracking

* **Historical Accuracy**
  * Prediction tracking
  * Information quality monitoring
  * Consistency evaluation
  * Expertise area mapping

* **Behavior Patterns**
  * Posting frequency analysis
  * Time pattern assessment
  * Content type distribution
  * Interaction network mapping

### Community Dynamics Analysis

* **Community Detection**
  * Social network clustering
  * Discussion group identification
  * Common interest mapping
  * Interaction pattern recognition

* **Narrative Tracking**
  * Narrative formation monitoring
  * Information flow analysis
  * Opinion leader identification
  * Consensus development tracking

* **Market Impact Correlation**
  * Social sentiment to price correlation
  * Volume relationship analysis
  * Volatility prediction indicators
  * Trading pattern relationships

## Integration with VeritasVault

### Social Data Pipeline

* **Collection frequency and methods**
* **Processing and enrichment steps**
* **Storage and indexing architecture**
* **Query and retrieval mechanisms**

### Social Intelligence Dashboard

* **Real-time monitoring features**
* **Historical trend visualization**
* **Entity relationship mapping**
* **Alert and notification system**

## Related Documentation

* [Market Sentiment Overview](./market-sentiment.md)
* [Market Sentiment Data Sources](./market-sentiment-sources.md)
* [News & Media Sources](./market-sentiment-sources-news.md)
* [Financial Disclosure Documents](./market-sentiment-sources-financial.md)
* [Alternative Data Sources](./market-sentiment-sources-alternative.md)
* [Market Sentiment NLP Techniques](./market-sentiment-nlp.md)

---

*Last Updated: 2025-05-29*