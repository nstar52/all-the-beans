import { useState, useEffect } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { getBeanOfTheDay } from '../services/api'
import type { Bean } from '../types/Bean'

function Home() {
  const [beanOfTheDay, setBeanOfTheDay] = useState<Bean | null>(null)
  const [loading, setLoading] = useState(true)
  const navigate = useNavigate()

  useEffect(() => {
    loadBeanOfTheDay()
  }, [])

  const loadBeanOfTheDay = async () => {
    try {
      const data = await getBeanOfTheDay()
      setBeanOfTheDay(data)
    } catch (error) {
      console.error('Failed to load bean of the day', error)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div>
      <h1 className="mb-4">All The Beans</h1>
      <p className="mb-5">Welcome to our coffee bean collection</p>

      {loading && <div className="text-center">Loading...</div>}

      {beanOfTheDay && (
        <div className="card mb-4">
          <div className="card-header bg-warning">
            <h3 className="mb-0">Bean of the Day</h3>
          </div>
          <div className="card-body">
            <div className="row">
              <div className="col-md-4">
                {beanOfTheDay.image && (
                  <img 
                    src={beanOfTheDay.image} 
                    className="img-fluid rounded" 
                    alt={beanOfTheDay.name}
                  />
                )}
              </div>
              <div className="col-md-8">
                <h4>{beanOfTheDay.name}</h4>
                <p className="text-muted">{beanOfTheDay.country}</p>
                <p><strong>Price:</strong> £{beanOfTheDay.cost.toFixed(2)}</p>
                <p>{beanOfTheDay.description}</p>
                <button 
                  className="btn btn-primary"
                  onClick={() => navigate(`/beans/${beanOfTheDay.id}`)}
                >
                  View Details
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

      <div className="mt-4">
        <Link to="/beans" className="btn btn-outline-primary">
          View All Beans
        </Link>
      </div>
    </div>
  )
}

export default Home

