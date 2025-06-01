import React, { useState, useEffect } from 'react';
import './App.css';

const App = () => {
  const [currentSlide, setCurrentSlide] = useState(0);
  const [selectedCategory, setSelectedCategory] = useState('All');
  const [currentView, setCurrentView] = useState('home'); // 'home' or 'course-detail'
  const [selectedCourse, setSelectedCourse] = useState(null);

  // Sample course data with PKR pricing for Pakistan
  const courses = [
    {
      id: 1,
      title: "AI Prompt Engineering Mastery",
      category: "AI Tools",
      price: "Rs. 8,999",
      originalPrice: "Rs. 12,999",
      rating: 4.8,
      students: "12,847",
      image: "https://images.unsplash.com/photo-1646583288948-24548aedffd8",
      duration: "8 hours",
      level: "Advanced",
      instructor: {
        name: "Dr. Ahmad Khan",
        title: "AI Research Scientist",
        image: "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d",
        students: "45,230",
        courses: 12
      },
      description: "Master the art of AI prompt engineering and unlock the full potential of ChatGPT, GPT-4, and other language models. Learn advanced techniques used by top AI professionals.",
      videoUrl: "https://www.youtube.com/embed/dQw4w9WgXcQ",
      curriculum: [
        { title: "Introduction to Prompt Engineering", duration: "45 min", completed: false },
        { title: "Understanding AI Language Models", duration: "60 min", completed: false },
        { title: "Advanced Prompting Techniques", duration: "90 min", completed: false },
        { title: "Business Applications", duration: "75 min", completed: false },
        { title: "Prompt Optimization Strategies", duration: "80 min", completed: false },
        { title: "Real-world Projects", duration: "120 min", completed: false }
      ],
      reviews: [
        { name: "Fatima Ahmed", rating: 5, comment: "Incredible course! Transformed my AI workflow completely.", time: "2 days ago" },
        { name: "Hassan Ali", rating: 5, comment: "Best investment I made for my career. Highly recommended!", time: "1 week ago" },
        { name: "Ayesha Khan", rating: 4, comment: "Very practical and up-to-date content. Great instructor!", time: "2 weeks ago" }
      ]
    },
    {
      id: 2,
      title: "YouTube Automation Empire",
      category: "YouTube Automation",
      price: "Rs. 7,499",
      originalPrice: "Rs. 10,999",
      rating: 4.9,
      students: "8,934",
      image: "https://images.unsplash.com/photo-1607799279861-4dd421887fb3",
      duration: "12 hours",
      level: "Intermediate",
      instructor: {
        name: "Usman Sheikh",
        title: "YouTube Growth Expert",
        image: "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e",
        students: "32,150",
        courses: 8
      },
      description: "Build automated YouTube channels that generate passive income. Learn the complete system from content creation to monetization.",
      videoUrl: "https://www.youtube.com/embed/dQw4w9WgXcQ",
      curriculum: [
        { title: "YouTube Automation Fundamentals", duration: "60 min", completed: false },
        { title: "Content Strategy & Planning", duration: "90 min", completed: false },
        { title: "Automated Video Creation", duration: "120 min", completed: false },
        { title: "SEO & Algorithm Optimization", duration: "75 min", completed: false },
        { title: "Monetization Strategies", duration: "90 min", completed: false },
        { title: "Scaling Your Channels", duration: "105 min", completed: false }
      ],
      reviews: [
        { name: "Bilal Ahmad", rating: 5, comment: "Started my first automated channel and it's already earning!", time: "3 days ago" },
        { name: "Zara Malik", rating: 5, comment: "Step-by-step process that actually works. Amazing!", time: "5 days ago" },
        { name: "Omar Hassan", rating: 4, comment: "Very detailed and practical course. Worth every rupee!", time: "1 week ago" }
      ]
    },
    {
      id: 3,
      title: "ChatGPT Business Automation",
      category: "ChatGPT Mastery",
      price: "Rs. 5,999",
      originalPrice: "Rs. 8,999",
      rating: 4.7,
      students: "15,672",
      image: "https://images.pexels.com/photos/3861969/pexels-photo-3861969.jpeg",
      duration: "6 hours",
      level: "Beginner",
      instructor: {
        name: "Sana Tariq",
        title: "Business Automation Specialist",
        image: "https://images.unsplash.com/photo-1494790108755-2616b612b786",
        students: "28,945",
        courses: 15
      },
      description: "Automate your business processes using ChatGPT and AI tools. Save hours of work and boost productivity exponentially.",
      videoUrl: "https://www.youtube.com/embed/dQw4w9WgXcQ",
      curriculum: [
        { title: "ChatGPT for Business Basics", duration: "45 min", completed: false },
        { title: "Automation Workflows", duration: "75 min", completed: false },
        { title: "Customer Service Automation", duration: "60 min", completed: false },
        { title: "Content Creation Automation", duration: "90 min", completed: false },
        { title: "Data Analysis with AI", duration: "70 min", completed: false },
        { title: "Building AI-Powered Solutions", duration: "100 min", completed: false }
      ],
      reviews: [
        { name: "Rashid Ali", rating: 5, comment: "Automated my entire content pipeline. Incredible results!", time: "1 day ago" },
        { name: "Mariam Khan", rating: 4, comment: "Great for beginners. Easy to follow and implement.", time: "4 days ago" },
        { name: "Tariq Ahmed", rating: 5, comment: "Boosted my business efficiency by 300%. Highly recommended!", time: "1 week ago" }
      ]
    },
    {
      id: 4,
      title: "Freelancing with AI Tools",
      category: "Freelancing",
      price: "Rs. 8,399",
      originalPrice: "Rs. 11,999",
      rating: 4.6,
      students: "9,421",
      image: "https://images.pexels.com/photos/1089438/pexels-photo-1089438.jpeg",
      duration: "10 hours",
      level: "Intermediate",
      instructor: {
        name: "Ahsan Malik",
        title: "Freelance Success Coach",
        image: "https://images.unsplash.com/photo-1560250097-0b93528c311a",
        students: "22,380",
        courses: 10
      },
      description: "Build a successful freelancing career using cutting-edge AI tools. Learn to deliver high-quality work faster and earn more.",
      videoUrl: "https://www.youtube.com/embed/dQw4w9WgXcQ",
      curriculum: [
        { title: "AI Tools for Freelancers", duration: "60 min", completed: false },
        { title: "Building Your Portfolio", duration: "90 min", completed: false },
        { title: "Client Acquisition Strategies", duration: "75 min", completed: false },
        { title: "Pricing and Negotiation", duration: "80 min", completed: false },
        { title: "Workflow Optimization", duration: "85 min", completed: false },
        { title: "Scaling Your Business", duration: "110 min", completed: false }
      ],
      reviews: [
        { name: "Farhana Sheikh", rating: 5, comment: "Tripled my freelancing income in just 2 months!", time: "2 days ago" },
        { name: "Kamran Ali", rating: 4, comment: "Solid strategies that work. Great mentorship approach.", time: "6 days ago" },
        { name: "Samina Ahmed", rating: 5, comment: "Best freelancing course I've ever taken. Life-changing!", time: "1 week ago" }
      ]
    },
    {
      id: 5,
      title: "Virtual Reality Development",
      category: "AI Tools",
      price: "Rs. 10,499",
      originalPrice: "Rs. 14,999",
      rating: 4.9,
      students: "5,847",
      image: "https://images.unsplash.com/photo-1605647540924-852290f6b0d5",
      duration: "15 hours",
      level: "Advanced",
      instructor: {
        name: "Hassan Shah",
        title: "VR/AR Development Expert",
        image: "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7",
        students: "18,235",
        courses: 7
      },
      description: "Master Virtual Reality development from scratch. Build immersive VR experiences and enter the future of technology.",
      videoUrl: "https://www.youtube.com/embed/dQw4w9WgXcQ",
      curriculum: [
        { title: "VR Development Fundamentals", duration: "90 min", completed: false },
        { title: "Unity 3D for VR", duration: "120 min", completed: false },
        { title: "VR Interaction Design", duration: "105 min", completed: false },
        { title: "Advanced VR Programming", duration: "135 min", completed: false },
        { title: "Optimization Techniques", duration: "90 min", completed: false },
        { title: "Publishing VR Applications", duration: "60 min", completed: false }
      ],
      reviews: [
        { name: "Ali Raza", rating: 5, comment: "Comprehensive VR course. Built my first VR app!", time: "3 days ago" },
        { name: "Nida Khan", rating: 5, comment: "Excellent instructor and cutting-edge content.", time: "5 days ago" },
        { name: "Salman Ahmed", rating: 4, comment: "Complex topics made simple. Great practical projects.", time: "1 week ago" }
      ]
    }
  ];

  const categories = ['All', 'AI Tools', 'YouTube Automation', 'ChatGPT Mastery', 'Freelancing'];

  const filteredCourses = selectedCategory === 'All' 
    ? courses 
    : courses.filter(course => course.category === selectedCategory);

  // Auto-slide for hero carousel
  useEffect(() => {
    if (currentView === 'home') {
      const timer = setInterval(() => {
        setCurrentSlide((prev) => (prev + 1) % courses.length);
      }, 4000);
      return () => clearInterval(timer);
    }
  }, [courses.length, currentView]);

  const handleCourseClick = (course) => {
    setSelectedCourse(course);
    setCurrentView('course-detail');
    window.scrollTo(0, 0);
  };

  const handleBackToHome = () => {
    setCurrentView('home');
    setSelectedCourse(null);
  };

  const CourseCard = ({ course, featured = false }) => (
    <div 
      className={`course-card ${featured ? 'featured-card' : ''}`}
      onClick={() => handleCourseClick(course)}
    >
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
          <div className="price-container">
            <span className="course-price">{course.price}</span>
            <span className="original-price">{course.originalPrice}</span>
          </div>
          <button className="enroll-btn" onClick={(e) => { e.stopPropagation(); handleCourseClick(course); }}>
            View Details
          </button>
        </div>
      </div>
    </div>
  );

  const CourseDetailPage = ({ course }) => (
    <div className="course-detail-page">
      {/* Navigation */}
      <nav className="navbar">
        <div className="nav-container">
          <div className="nav-logo" onClick={handleBackToHome} style={{ cursor: 'pointer' }}>
            <span className="logo-text">SkillNest</span>
            <span className="logo-tagline">Future Learning Platform</span>
          </div>
          <div className="nav-menu">
            <button className="back-btn" onClick={handleBackToHome}>
              ← Back to Courses
            </button>
          </div>
          <div className="nav-actions">
            <button className="login-btn">Login</button>
            <button className="signup-btn">Sign Up</button>
          </div>
        </div>
      </nav>

      {/* Course Hero Section */}
      <section className="course-hero">
        <div className="container">
          <div className="course-hero-content">
            <div className="course-info-left">
              <div className="course-breadcrumb">
                <span>Courses</span> → <span>{course.category}</span> → <span>{course.title}</span>
              </div>
              <h1 className="course-detail-title">{course.title}</h1>
              <p className="course-description">{course.description}</p>
              
              <div className="course-meta">
                <div className="meta-item">
                  <span className="meta-label">Rating:</span>
                  <span className="meta-value">⭐ {course.rating} ({course.students} students)</span>
                </div>
                <div className="meta-item">
                  <span className="meta-label">Duration:</span>
                  <span className="meta-value">{course.duration}</span>
                </div>
                <div className="meta-item">
                  <span className="meta-label">Level:</span>
                  <span className="meta-value">{course.level}</span>
                </div>
                <div className="meta-item">
                  <span className="meta-label">Category:</span>
                  <span className="meta-value">{course.category}</span>
                </div>
              </div>

              <div className="course-pricing">
                <span className="current-price">{course.price}</span>
                <span className="original-price">{course.originalPrice}</span>
                <span className="discount">Save {Math.round(((parseInt(course.originalPrice.replace(/[^\d]/g, '')) - parseInt(course.price.replace(/[^\d]/g, ''))) / parseInt(course.originalPrice.replace(/[^\d]/g, ''))) * 100)}%</span>
              </div>

              <button className="enroll-now-btn">
                <span>🚀 Enroll Now</span>
              </button>
            </div>

            <div className="course-video-container">
              <div className="video-wrapper">
                <iframe
                  src={course.videoUrl}
                  title={course.title}
                  frameBorder="0"
                  allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                  allowFullScreen
                ></iframe>
                <div className="video-overlay">
                  <div className="play-button">▶</div>
                  <div className="video-info">
                    <span>Preview Course</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Course Content Sections */}
      <section className="course-content-section">
        <div className="container">
          <div className="content-grid">
            {/* Curriculum */}
            <div className="curriculum-section">
              <h2 className="section-title">Course Curriculum</h2>
              <div className="curriculum-list">
                {course.curriculum.map((lesson, index) => (
                  <div key={index} className="curriculum-item">
                    <div className="lesson-number">{index + 1}</div>
                    <div className="lesson-content">
                      <h4 className="lesson-title">{lesson.title}</h4>
                      <span className="lesson-duration">{lesson.duration}</span>
                    </div>
                    <div className="lesson-status">
                      {lesson.completed ? '✅' : '🔒'}
                    </div>
                  </div>
                ))}
              </div>
            </div>

            {/* Instructor Info */}
            <div className="instructor-section">
              <h2 className="section-title">Your Instructor</h2>
              <div className="instructor-card">
                <img src={course.instructor.image} alt={course.instructor.name} className="instructor-image" />
                <div className="instructor-info">
                  <h3 className="instructor-name">{course.instructor.name}</h3>
                  <p className="instructor-title">{course.instructor.title}</p>
                  <div className="instructor-stats">
                    <div className="stat">
                      <span className="stat-number">{course.instructor.students}</span>
                      <span className="stat-label">Students</span>
                    </div>
                    <div className="stat">
                      <span className="stat-number">{course.instructor.courses}</span>
                      <span className="stat-label">Courses</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Reviews Section */}
          <div className="reviews-section">
            <h2 className="section-title">Student Reviews</h2>
            <div className="reviews-grid">
              {course.reviews.map((review, index) => (
                <div key={index} className="review-card">
                  <div className="review-header">
                    <div className="reviewer-info">
                      <span className="reviewer-name">{review.name}</span>
                      <span className="review-time">{review.time}</span>
                    </div>
                    <div className="review-rating">
                      {'⭐'.repeat(review.rating)}
                    </div>
                  </div>
                  <p className="review-comment">"{review.comment}"</p>
                </div>
              ))}
            </div>
          </div>
        </div>
      </section>
    </div>
  );

  if (currentView === 'course-detail' && selectedCourse) {
    return <CourseDetailPage course={selectedCourse} />;
  }

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
              <p className="footer-desc">Empowering Pakistan's next generation with future-ready skills</p>
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
            <p>&copy; 2025 SkillNest. Shaping the Future of Learning in Pakistan.</p>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default App;