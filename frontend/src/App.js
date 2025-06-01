import React, { useState, useEffect } from 'react';
import './App.css';

const App = () => {
  const [currentSlide, setCurrentSlide] = useState(0);
  const [selectedCategory, setSelectedCategory] = useState('All');

  // Sample course data with futuristic themes
  const courses = [
    {
      id: 1,
      title: "AI Prompt Engineering Mastery",
      category: "AI Tools",
      price: "₹299",
      rating: 4.8,
      students: "12,847",
      image: "https://images.unsplash.com/photo-1646583288948-24548aedffd8",
      duration: "8 hours",
      level: "Advanced"
    },
    {
      id: 2,
      title: "YouTube Automation Empire",
      category: "YouTube Automation",
      price: "₹249",
      rating: 4.9,
      students: "8,934",
      image: "https://images.unsplash.com/photo-1607799279861-4dd421887fb3",
      duration: "12 hours",
      level: "Intermediate"
    },
    {
      id: 3,
      title: "ChatGPT Business Automation",
      category: "ChatGPT Mastery",
      price: "₹199",
      rating: 4.7,
      students: "15,672",
      image: "https://images.pexels.com/photos/3861969/pexels-photo-3861969.jpeg",
      duration: "6 hours",
      level: "Beginner"
    },
    {
      id: 4,
      title: "Freelancing with AI Tools",
      category: "Freelancing",
      price: "₹279",
      rating: 4.6,
      students: "9,421",
      image: "https://images.pexels.com/photos/1089438/pexels-photo-1089438.jpeg",
      duration: "10 hours",
      level: "Intermediate"
    },
    {
      id: 5,
      title: "Virtual Reality Development",
      category: "AI Tools",
      price: "₹349",
      rating: 4.9,
      students: "5,847",
      image: "https://images.unsplash.com/photo-1605647540924-852290f6b0d5",
      duration: "15 hours",
      level: "Advanced"
    }
  ];

  const categories = ['All', 'AI Tools', 'YouTube Automation', 'ChatGPT Mastery', 'Freelancing'];

  const filteredCourses = selectedCategory === 'All' 
    ? courses 
    : courses.filter(course => course.category === selectedCategory);

  // Auto-slide for hero carousel
  useEffect(() => {
    const timer = setInterval(() => {
      setCurrentSlide((prev) => (prev + 1) % courses.length);
    }, 4000);
    return () => clearInterval(timer);
  }, [courses.length]);

  const CourseCard = ({ course, featured = false }) => (
    <div className={`course-card ${featured ? 'featured-card' : ''}`}>
      <div className="course-image-container">
        <img src={course.image} alt={course.title} className="course-image" />
        <div className="course-overlay">
          <div className="course-level">{course.level}</div>
          <div className="course-duration">{course.duration}</div>
        </div>
      </div>
      <div className="course-content">
        <div className="course-category">{course.category}</div>
        <h3 className="course-title">{course.title}</h3>
        <div className="course-stats">
          <span className="rating">
            ⭐ {course.rating}
          </span>
          <span className="students">{course.students} students</span>
        </div>
        <div className="course-footer">
          <span className="course-price">{course.price}</span>
          <button className="enroll-btn">Enroll Now</button>
        </div>
      </div>
    </div>
  );

  return (
    <div className="app">
      {/* Navigation */}
      <nav className="navbar">
        <div className="nav-container">
          <div className="nav-logo">
            <span className="logo-text">SkillNest</span>
            <span className="logo-tagline">Future Learning Platform</span>
          </div>
          <div className="nav-menu">
            <a href="#home" className="nav-link active">Home</a>
            <a href="#courses" className="nav-link">Courses</a>
            <a href="#about" className="nav-link">About</a>
            <a href="#contact" className="nav-link">Contact</a>
          </div>
          <div className="nav-actions">
            <button className="login-btn">Login</button>
            <button className="signup-btn">Sign Up</button>
          </div>
        </div>
      </nav>

      {/* Hero Section */}
      <section className="hero-section" id="home">
        <div className="hero-bg">
          <img 
            src="https://images.unsplash.com/photo-1660165458059-57cfb6cc87e5" 
            alt="Futuristic background" 
            className="hero-bg-image"
          />
          <div className="hero-overlay"></div>
        </div>
        <div className="hero-content">
          <div className="hero-text">
            <h1 className="hero-title">
              Learn <span className="gradient-text">Future Skills</span> Today
            </h1>
            <p className="hero-subtitle">
              Master AI, automation, and next-gen technologies with our immersive learning platform
            </p>
            <div className="search-container">
              <input 
                type="text" 
                placeholder="Search for courses, skills, or technologies..."
                className="search-input"
              />
              <button className="search-btn">
                <span>🔍</span>
              </button>
            </div>
          </div>
          <div className="hero-stats">
            <div className="stat-item">
              <span className="stat-number">50K+</span>
              <span className="stat-label">Students</span>
            </div>
            <div className="stat-item">
              <span className="stat-number">200+</span>
              <span className="stat-label">Courses</span>
            </div>
            <div className="stat-item">
              <span className="stat-number">98%</span>
              <span className="stat-label">Success Rate</span>
            </div>
          </div>
        </div>
      </section>

      {/* Featured Courses Carousel */}
      <section className="featured-section">
        <div className="container">
          <h2 className="section-title">
            <span className="gradient-text">Trending</span> Courses
          </h2>
          <div className="carousel-container">
            <div className="carousel-track" style={{ transform: `translateX(-${currentSlide * 100}%)` }}>
              {courses.map((course, index) => (
                <div key={course.id} className="carousel-slide">
                  <CourseCard course={course} featured={true} />
                </div>
              ))}
            </div>
            <div className="carousel-indicators">
              {courses.map((_, index) => (
                <button
                  key={index}
                  className={`indicator ${index === currentSlide ? 'active' : ''}`}
                  onClick={() => setCurrentSlide(index)}
                />
              ))}
            </div>
          </div>
        </div>
      </section>

      {/* Categories Section */}
      <section className="categories-section">
        <div className="container">
          <h2 className="section-title">
            Explore <span className="gradient-text">Categories</span>
          </h2>
          <div className="categories-grid">
            {categories.slice(1).map((category) => (
              <div key={category} className="category-card">
                <div className="category-icon">
                  {category === 'AI Tools' && '🤖'}
                  {category === 'YouTube Automation' && '📹'}
                  {category === 'ChatGPT Mastery' && '💬'}
                  {category === 'Freelancing' && '💼'}
                </div>
                <h3 className="category-title">{category}</h3>
                <p className="category-count">
                  {courses.filter(c => c.category === category).length} courses
                </p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Course Catalog */}
      <section className="catalog-section" id="courses">
        <div className="container">
          <h2 className="section-title">
            All <span className="gradient-text">Courses</span>
          </h2>
          <div className="catalog-filters">
            {categories.map((category) => (
              <button
                key={category}
                className={`filter-btn ${selectedCategory === category ? 'active' : ''}`}
                onClick={() => setSelectedCategory(category)}
              >
                {category}
              </button>
            ))}
          </div>
          <div className="courses-grid">
            {filteredCourses.map((course) => (
              <CourseCard key={course.id} course={course} />
            ))}
          </div>
        </div>
      </section>

      {/* AI Assistant Floating Button */}
      <div className="ai-assistant">
        <button className="ai-btn">
          <span className="ai-icon">🤖</span>
          <span className="ai-text">AI Assistant</span>
        </button>
      </div>

      {/* Footer */}
      <footer className="footer">
        <div className="container">
          <div className="footer-content">
            <div className="footer-section">
              <h3 className="footer-title">SkillNest</h3>
              <p className="footer-desc">Empowering the next generation with future-ready skills</p>
            </div>
            <div className="footer-section">
              <h4 className="footer-heading">Platform</h4>
              <ul className="footer-links">
                <li><a href="#courses">Courses</a></li>
                <li><a href="#pricing">Pricing</a></li>
                <li><a href="#about">About</a></li>
              </ul>
            </div>
            <div className="footer-section">
              <h4 className="footer-heading">Support</h4>
              <ul className="footer-links">
                <li><a href="#help">Help Center</a></li>
                <li><a href="#contact">Contact</a></li>
                <li><a href="#community">Community</a></li>
              </ul>
            </div>
          </div>
          <div className="footer-bottom">
            <p>&copy; 2025 SkillNest. Shaping the Future of Learning.</p>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default App;